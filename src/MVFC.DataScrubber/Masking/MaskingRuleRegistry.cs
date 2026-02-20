namespace MVFC.DataScrubber.Masking;

using MVFC.DataScrubber.Core.Abstractions;
using MVFC.DataScrubber.Masking.Rules;
using System.Collections.Frozen;

/// <summary>
/// Registry for masking rules, providing a centralized lookup by rule ID.
/// </summary>
public sealed class MaskingRuleRegistry
{
    private readonly Dictionary<string, IMaskingRule> _rules = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="MaskingRuleRegistry"/> class with default Brazilian-focused rules.
    /// </summary>
    public MaskingRuleRegistry()
    {
        RegisterDefaults();
    }

    /// <summary>
    /// Registers a custom masking rule.
    /// </summary>
    /// <param name="rule">The masking rule to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when rule is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a rule with the same ID already exists.</exception>
    public void Register(IMaskingRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        if (_rules.ContainsKey(rule.RuleId))
            throw new InvalidOperationException($"A rule with ID '{rule.RuleId}' is already registered.");

        _rules[rule.RuleId] = rule;
    }

    /// <summary>
    /// Registers a custom masking rule, replacing any existing rule with the same ID.
    /// </summary>
    /// <param name="rule">The masking rule to register.</param>
    /// <exception cref="ArgumentNullException">Thrown when rule is null.</exception>
    public void RegisterOrReplace(IMaskingRule rule)
    {
        if (rule == null)
            throw new ArgumentNullException(nameof(rule));

        _rules[rule.RuleId] = rule;
    }

    /// <summary>
    /// Attempts to get a masking rule by its ID.
    /// </summary>
    /// <param name="ruleId">The rule ID.</param>
    /// <param name="rule">The found rule, or null if not found.</param>
    /// <returns>True if the rule was found; otherwise, false.</returns>
    public bool TryGet(string ruleId, out IMaskingRule? rule)
    {
        return _rules.TryGetValue(ruleId ?? string.Empty, out rule);
    }

    /// <summary>
    /// Gets a masking rule by its ID.
    /// </summary>
    /// <param name="ruleId">The rule ID.</param>
    /// <returns>The found rule.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the rule ID is not found.</exception>
    public IMaskingRule Get(string ruleId)
    {
        if (!_rules.TryGetValue(ruleId ?? string.Empty, out var rule))
            throw new KeyNotFoundException($"Masking rule with ID '{ruleId}' not found in registry.");

        return rule;
    }

    /// <summary>
    /// Gets all registered rules as a frozen collection.
    /// </summary>
    public IReadOnlyDictionary<string, IMaskingRule> GetAll() => _rules.ToFrozenDictionary();

    /// <summary>
    /// Registers all default masking rules.
    /// </summary>
    private void RegisterDefaults()
    {
        // Brazilian documents
        Register(Mask.Cpf());
        Register(Mask.Cnpj());
        
        // Text/Personal data
        Register(Mask.Name());
        Register(Mask.Email());
        Register(Mask.EmailPreservingDomain());
        Register(Mask.PhoneBr());
        Register(Mask.Address());
        
        // Financial data
        Register(Mask.CreditCardPartial());
        Register(Mask.CreditCardFull());
        
        // Generic
        Register(Mask.Url());
        Register(Mask.Placeholder());
        Register(Mask.Hash());
        Register(Mask.NumericId());
        Register(Mask.Guid());
        Register(Mask.Null());
        Register(Mask.Identity());
    }
}
