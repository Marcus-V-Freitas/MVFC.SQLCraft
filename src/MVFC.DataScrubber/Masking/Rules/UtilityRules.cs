namespace MVFC.DataScrubber.Masking.Rules;

using MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Masking rule for credit card numbers (showing only last 4 digits).
/// </summary>
internal sealed class CreditCardPartialMaskingRule : BaseMaskingRule
{
    public override string RuleId => "credit_card_partial";
    public override string DisplayName => "Credit Card (Last 4 Digits)";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        var numeric = new string(str.Where(char.IsDigit).ToArray());

        if (numeric.Length < 4)
            return "****";

        var lastFour = numeric[^4..];
        return $"**** **** **** {lastFour}";
    }
}

/// <summary>
/// Masking rule for completely masking credit card numbers.
/// </summary>
internal sealed class CreditCardFullMaskingRule : BaseMaskingRule
{
    public override string RuleId => "credit_card_full";
    public override string DisplayName => "Credit Card (Full Mask)";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        return "**** **** **** ****";
    }
}

/// <summary>
/// Masking rule for URLs.
/// </summary>
internal sealed class UrlMaskingRule : BaseMaskingRule
{
    public override string RuleId => "url";
    public override string DisplayName => "URL";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        return "https://example.com/resource";
    }
}

/// <summary>
/// Masking rule that replaces text with a fixed placeholder.
/// </summary>
internal sealed class PlaceholderMaskingRule : BaseMaskingRule
{
    private readonly string _placeholder;

    public PlaceholderMaskingRule(string placeholder = "[MASKED]")
    {
        _placeholder = placeholder ?? "[MASKED]";
    }

    public override string RuleId => "placeholder";
    public override string DisplayName => "Placeholder";

    public override object? Mask(object? value, MaskingContext context)
    {
        return IsNullOrEmpty(value) ? null : _placeholder;
    }
}

/// <summary>
/// Masking rule that replaces text with hashes, preserving length.
/// </summary>
internal sealed class HashMaskingRule : BaseMaskingRule
{
    public override string RuleId => "hash";
    public override string DisplayName => "Hash";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        var str = value!.ToString()!;
        return new string('*', str.Length);
    }
}

/// <summary>
/// Masking rule for numeric IDs while preserving uniqueness.
/// </summary>
internal sealed class NumericIdMaskingRule : BaseMaskingRule
{
    public override string RuleId => "numeric_id";
    public override string DisplayName => "Numeric ID";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        if (!long.TryParse(value!.ToString(), out _))
            return value;

        // Generate a random numeric ID
        var random = new Random();
        return random.Next(100000, int.MaxValue);
    }
}

/// <summary>
/// Masking rule for GUIDs/UUIDs.
/// </summary>
internal sealed class GuidMaskingRule : BaseMaskingRule
{
    public override string RuleId => "guid";
    public override string DisplayName => "GUID";

    public override object? Mask(object? value, MaskingContext context)
    {
        if (IsNullOrEmpty(value))
            return null;

        return Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Masking rule that nullifies the value.
/// </summary>
internal sealed class NullMaskingRule : BaseMaskingRule
{
    public override string RuleId => "null";
    public override string DisplayName => "Null";

    public override object? Mask(object? value, MaskingContext context) => null;
}

/// <summary>
/// Masking rule that leaves the value unchanged.
/// </summary>
internal sealed class IdentityMaskingRule : BaseMaskingRule
{
    public override string RuleId => "identity";
    public override string DisplayName => "Identity (No Change)";

    public override object? Mask(object? value, MaskingContext context) => value;
}
