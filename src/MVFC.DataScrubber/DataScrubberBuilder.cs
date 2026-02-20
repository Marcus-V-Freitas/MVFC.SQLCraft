namespace MVFC.DataScrubber;

using MVFC.DataScrubber.Core;
using MVFC.DataScrubber.Core.Abstractions;
using MVFC.DataScrubber.Masking;
using Microsoft.Extensions.Logging;

/// <summary>
/// Builder class for configuring and executing data scrubbing operations.
/// Provides a fluent API for defining scrub plans and executing anonymization.
/// </summary>
public sealed class DataScrubberBuilder
{
    private readonly MaskingRuleRegistry _ruleRegistry = new();
    private readonly List<IScrubberProvider> _providers = new();
    private readonly ILogger? _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataScrubberBuilder"/> class.
    /// </summary>
    /// <param name="logger">Optional logger for operation diagnostics.</param>
    public DataScrubberBuilder(ILogger? logger = null)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets the masking rule registry for custom rule management.
    /// </summary>
    public MaskingRuleRegistry RuleRegistry => _ruleRegistry;

    /// <summary>
    /// Adds a scrubber provider for a specific data source.
    /// </summary>
    /// <param name="provider">The scrubber provider to add.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when provider is null.</exception>
    public DataScrubberBuilder AddProvider(IScrubberProvider provider)
    {
        if (provider == null)
            throw new ArgumentNullException(nameof(provider));

        _providers.Add(provider);
        _logger?.LogInformation("Scrubber provider '{ProviderName}' added.", provider.ProviderName);
        return this;
    }

    /// <summary>
    /// Registers a custom masking rule.
    /// </summary>
    /// <param name="rule">The masking rule to register.</param>
    /// <returns>The current builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when rule is null.</exception>
    public DataScrubberBuilder WithMaskingRule(IMaskingRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        _ruleRegistry.RegisterOrReplace(rule);
        _logger?.LogInformation("Masking rule '{RuleId}' ({DisplayName}) registered.", rule.RuleId, rule.DisplayName);
        return this;
    }

    /// <summary>
    /// Executes the scrubbing operation for the given plan.
    /// </summary>
    /// <param name="scrubPlan">The plan defining the scrubbing targets and rules.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The result of the scrubbing execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when scrubPlan is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no provider matches the plan's source type.</exception>
    public async Task<ScrubExecutionResult> ExecuteAsync(ScrubPlan scrubPlan, CancellationToken cancellationToken = default)
    {
        if (scrubPlan == null)
            throw new ArgumentNullException(nameof(scrubPlan));

        var provider = _providers.FirstOrDefault(p => 
            p.ProviderName.Equals(scrubPlan.SourceType, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            var message = $"No provider found for source type '{scrubPlan.SourceType}'. Available providers: {string.Join(", ", _providers.Select(p => p.ProviderName))}";
            _logger?.LogError(message);
            throw new InvalidOperationException(message);
        }

        _logger?.LogInformation("Starting scrub execution for plan '{PlanId}' with {TargetCount} targets.", 
            scrubPlan.PlanId, scrubPlan.Targets.Count);

        var startTime = DateTimeOffset.UtcNow;
        try
        {
            var result = await provider.ExecuteAsync(scrubPlan, cancellationToken);
            var duration = DateTimeOffset.UtcNow - startTime;

            if (result.IsSuccess)
            {
                _logger?.LogInformation(
                    "Scrub execution completed successfully. Records scrubbed: {RecordsScrubbed}, Fields masked: {FieldsMasked}, Duration: {Duration}ms",
                    result.RecordsScrubbed, result.FieldsMasked, duration.TotalMilliseconds);
            }
            else
            {
                _logger?.LogError(
                    "Scrub execution failed. Error: {ErrorMessage}",
                    result.ErrorMessage);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Scrub execution threw an exception.");
            throw;
        }
    }

    /// <summary>
    /// Validates the configuration of all providers.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A validation result indicating configuration validity.</returns>
    public async Task<ScrubValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
    {
        if (_providers.Count == 0)
            return ScrubValidationResult.Failure("No providers configured.");

        var errors = new List<string>();

        foreach (var provider in _providers)
        {
            var result = await provider.ValidateAsync(cancellationToken);
            if (!result.IsValid)
                errors.AddRange(result.Errors);
        }

        if (errors.Count == 0)
        {
            _logger?.LogInformation("Validation successful. All providers are properly configured.");
            return ScrubValidationResult.Success();
        }

        _logger?.LogWarning("Validation failed with {ErrorCount} errors.", errors.Count);
        return ScrubValidationResult.Failure(errors.ToArray());
    }

    /// <summary>
    /// Creates a new scrub plan builder for fluent configuration.
    /// </summary>
    /// <param name="planId">The unique identifier for the plan.</param>
    /// <returns>A scrub plan builder instance.</returns>
    public ScrubPlanBuilder CreatePlan(string planId)
    {
        return new ScrubPlanBuilder(planId, _ruleRegistry);
    }
}
