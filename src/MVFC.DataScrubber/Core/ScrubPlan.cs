namespace MVFC.DataScrubber.Core;

/// <summary>
/// Represents a plan for scrubbing/anonymizing data sources.
/// </summary>
public sealed record ScrubPlan
{
    /// <summary>
    /// Gets the plan identifier.
    /// </summary>
    public required string PlanId { get; init; }

    /// <summary>
    /// Gets the plan description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the source type (SQL, MongoDB, etc.).
    /// </summary>
    public required string SourceType { get; init; }

    /// <summary>
    /// Gets the list of tables/collections to scrub.
    /// </summary>
    public required IReadOnlyList<ScrubTarget> Targets { get; init; }

    /// <summary>
    /// Gets optional filters to apply before scrubbing (WHERE clause or MongoDB filter).
    /// </summary>
    public string? Filter { get; init; }

    /// <summary>
    /// Gets a value indicating whether to use transactions during scrubbing.
    /// </summary>
    public bool UseTransactions { get; init; } = true;

    /// <summary>
    /// Gets the batch size for processing records.
    /// </summary>
    public int BatchSize { get; init; } = 1000;
}

/// <summary>
/// Represents a single table or collection as a target for scrubbing.
/// </summary>
public sealed record ScrubTarget
{
    /// <summary>
    /// Gets the schema name (for SQL databases).
    /// </summary>
    public string? Schema { get; init; }

    /// <summary>
    /// Gets the table or collection name.
    /// </summary>
    public required string TableOrCollection { get; init; }

    /// <summary>
    /// Gets the list of fields to mask in this table/collection.
    /// </summary>
    public required IReadOnlyList<FieldMaskingRule> Fields { get; init; }

    /// <summary>
    /// Gets an optional WHERE clause or MongoDB filter specific to this target.
    /// </summary>
    public string? Filter { get; init; }
}

/// <summary>
/// Defines which masking rule to apply to a specific field.
/// </summary>
public sealed record FieldMaskingRule
{
    /// <summary>
    /// Gets the field name to mask.
    /// </summary>
    public required string FieldName { get; init; }

    /// <summary>
    /// Gets the rule ID to apply for masking.
    /// </summary>
    public required string RuleId { get; init; }

    /// <summary>
    /// Gets optional metadata specific to this field's masking.
    /// </summary>
    public IReadOnlyDictionary<string, object?>? Metadata { get; init; }
}