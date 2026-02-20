namespace MVFC.DataScrubber.Core.Abstractions;

/// <summary>
/// Defines a contract for masking/anonymizing a value based on its type and context.
/// </summary>
public interface IMaskingRule
{
    /// <summary>
    /// Gets the unique identifier for this masking rule.
    /// </summary>
    string RuleId { get; }

    /// <summary>
    /// Gets the display name of this masking rule.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Determines whether this rule can handle the given value.
    /// </summary>
    /// <param name="value">The value to evaluate.</param>
    /// <returns>True if this rule can mask the value; otherwise, false.</returns>
    bool CanHandle(object? value);

    /// <summary>
    /// Applies the masking transformation to the provided value.
    /// </summary>
    /// <param name="value">The original value to mask.</param>
    /// <param name="context">Additional context information for masking (column name, table name, etc.).</param>
    /// <returns>The masked/anonymized value.</returns>
    object? Mask(object? value, MaskingContext context);
}

/// <summary>
/// Provides context information when applying masking rules.
/// </summary>
public sealed record MaskingContext
{
    /// <summary>
    /// Gets the source type (e.g., "SQL", "MongoDB", "CSV").
    /// </summary>
    public required string SourceType { get; init; }

    /// <summary>
    /// Gets the table or collection name where the field resides.
    /// </summary>
    public required string TableOrCollection { get; init; }

    /// <summary>
    /// Gets the column or field name being masked.
    /// </summary>
    public required string ColumnOrField { get; init; }

    /// <summary>
    /// Gets optional metadata for advanced masking scenarios.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Metadata { get; init; }
}