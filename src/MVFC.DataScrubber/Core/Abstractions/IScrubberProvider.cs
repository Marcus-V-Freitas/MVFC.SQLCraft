namespace MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Defines a contract for scrubbing/anonymizing data from a specific data source.
/// </summary>
public interface IScrubberProvider
{
    /// <summary>
    /// Gets the provider type name (e.g., "SqlServer", "PostgreSQL", "MongoDB").
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Validates whether the configuration is valid for this provider.
    /// </summary>
    /// <returns>A validation result indicating success or failure with error messages.</returns>
    Task<ScrubValidationResult> ValidateAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the scrubbing operation based on the provided scrub plan.
    /// </summary>
    /// <param name="scrubPlan">The plan defining which tables/collections and fields to scrub.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A result indicating success or failure with statistics about the scrub operation.</returns>
    Task<ScrubExecutionResult> ExecuteAsync(ScrubPlan scrubPlan, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the validation result of a scrubber provider configuration.
/// </summary>
public sealed record ScrubValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the validation was successful.
    /// </summary>
    public required bool IsValid { get; init; }

    /// <summary>
    /// Gets the list of validation errors, if any.
    /// </summary>
    public required IReadOnlyList<string> Errors { get; init; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ScrubValidationResult Success() => new() { IsValid = true, Errors = Array.Empty<string>() };

    /// <summary>
    /// Creates a failed validation result with error messages.
    /// </summary>
    public static ScrubValidationResult Failure(params string[] errors) => new() { IsValid = false, Errors = errors };
}

/// <summary>
/// Represents the result of a scrubbing execution.
/// </summary>
public sealed record ScrubExecutionResult
{
    /// <summary>
    /// Gets a value indicating whether the execution was successful.
    /// </summary>
    public required bool IsSuccess { get; init; }

    /// <summary>
    /// Gets the number of records scrubbed.
    /// </summary>
    public required long RecordsScrubbed { get; init; }

    /// <summary>
    /// Gets the number of fields masked.
    /// </summary>
    public required long FieldsMasked { get; init; }

    /// <summary>
    /// Gets the execution duration.
    /// </summary>
    public required TimeSpan Duration { get; init; }

    /// <summary>
    /// Gets any error message if the execution failed.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Creates a successful execution result.
    /// </summary>
    public static ScrubExecutionResult Success(long recordsScrubbed, long fieldsMasked, TimeSpan duration) => new()
    {
        IsSuccess = true,
        RecordsScrubbed = recordsScrubbed,
        FieldsMasked = fieldsMasked,
        Duration = duration
    };

    /// <summary>
    /// Creates a failed execution result.
    /// </summary>
    public static ScrubExecutionResult Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        RecordsScrubbed = 0,
        FieldsMasked = 0,
        Duration = TimeSpan.Zero,
        ErrorMessage = errorMessage
    };
}