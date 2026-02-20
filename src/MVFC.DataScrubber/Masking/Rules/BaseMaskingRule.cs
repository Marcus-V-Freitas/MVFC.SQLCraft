namespace MVFC.DataScrubber.Masking.Rules;

using MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Base class for implementing masking rules.
/// </summary>
public abstract class BaseMaskingRule : IMaskingRule
{
    /// <inheritdoc />
    public abstract string RuleId { get; }

    /// <inheritdoc />
    public abstract string DisplayName { get; }

    /// <inheritdoc />
    public virtual bool CanHandle(object? value) => true;

    /// <inheritdoc />
    public abstract object? Mask(object? value, MaskingContext context);

    /// <summary>
    /// Checks if the value is null or empty.
    /// </summary>
    protected static bool IsNullOrEmpty(object? value) => value == null || (value is string str && string.IsNullOrWhiteSpace(str));

    /// <summary>
    /// Generates a random alphanumeric string.
    /// </summary>
    protected static string GenerateRandomString(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }

    /// <summary>
    /// Generates a random numeric string.
    /// </summary>
    protected static string GenerateRandomNumeric(int length = 10)
    {
        const string chars = "0123456789";
        var random = new Random();
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }
}
