namespace MVFC.DataScrubber;

using MVFC.DataScrubber.Core;
using MVFC.DataScrubber.Masking;

/// <summary>
/// Fluent builder for creating scrub plans with a declarative syntax.
/// </summary>
public sealed class ScrubPlanBuilder
{
    private readonly string _planId;
    private readonly MaskingRuleRegistry _ruleRegistry;
    private string? _description;
    private string? _sourceType;
    private readonly List<ScrubTarget> _targets = new();
    private string? _filter;
    private bool _useTransactions = true;
    private int _batchSize = 1000;

    internal ScrubPlanBuilder(string planId, MaskingRuleRegistry ruleRegistry)
    {
        _planId = planId ?? throw new ArgumentNullException(nameof(planId));
        _ruleRegistry = ruleRegistry ?? throw new ArgumentNullException(nameof(ruleRegistry));
    }

    /// <summary>
    /// Sets the description for this scrub plan.
    /// </summary>
    public ScrubPlanBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    /// <summary>
    /// Sets the source type (e.g., "SqlServer", "PostgreSQL", "MongoDB").
    /// </summary>
    public ScrubPlanBuilder ForSource(string sourceType)
    {
        _sourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
        return this;
    }

    /// <summary>
    /// Sets a global filter to apply before scrubbing (e.g., WHERE clause for SQL).
    /// </summary>
    public ScrubPlanBuilder WithFilter(string filter)
    {
        _filter = filter;
        return this;
    }

    /// <summary>
    /// Sets whether to use transactions during scrubbing.
    /// </summary>
    public ScrubPlanBuilder UseTransactions(bool useTransactions = true)
    {
        _useTransactions = useTransactions;
        return this;
    }

    /// <summary>
    /// Sets the batch size for processing records.
    /// </summary>
    public ScrubPlanBuilder WithBatchSize(int batchSize)
    {
        if (batchSize <= 0)
            throw new ArgumentException("Batch size must be greater than 0.", nameof(batchSize));

        _batchSize = batchSize;
        return this;
    }

    /// <summary>
    /// Adds a table/collection target to the scrub plan.
    /// </summary>
    /// <param name="tableOrCollection">The table or collection name.</param>
    /// <param name="configureTarget">Configuration delegate for the target.</param>
    public ScrubPlanBuilder AddTarget(string tableOrCollection, Action<ScrubTargetBuilder> configureTarget)
    {
        if (string.IsNullOrWhiteSpace(tableOrCollection))
            throw new ArgumentException("Table or collection name cannot be empty.", nameof(tableOrCollection));

        if (configureTarget == null)
            throw new ArgumentNullException(nameof(configureTarget));

        var targetBuilder = new ScrubTargetBuilder(tableOrCollection, _ruleRegistry);
        configureTarget(targetBuilder);
        _targets.Add(targetBuilder.Build());

        return this;
    }

    /// <summary>
    /// Builds the scrub plan.
    /// </summary>
    public ScrubPlan Build()
    {
        if (string.IsNullOrWhiteSpace(_sourceType))
            throw new InvalidOperationException("Source type must be specified using ForSource().");

        if (_targets.Count == 0)
            throw new InvalidOperationException("At least one target must be added using AddTarget().");

        return new ScrubPlan
        {
            PlanId = _planId,
            Description = _description,
            SourceType = _sourceType,
            Targets = _targets.AsReadOnly(),
            Filter = _filter,
            UseTransactions = _useTransactions,
            BatchSize = _batchSize
        };
    }
}

/// <summary>
/// Fluent builder for configuring individual scrub targets.
/// </summary>
public sealed class ScrubTargetBuilder
{
    private readonly string _tableOrCollection;
    private readonly MaskingRuleRegistry _ruleRegistry;
    private string? _schema;
    private string? _filter;
    private readonly List<FieldMaskingRule> _fields = new();

    internal ScrubTargetBuilder(string tableOrCollection, MaskingRuleRegistry ruleRegistry)
    {
        _tableOrCollection = tableOrCollection ?? throw new ArgumentNullException(nameof(tableOrCollection));
        _ruleRegistry = ruleRegistry ?? throw new ArgumentNullException(nameof(ruleRegistry));
    }

    /// <summary>
    /// Sets the schema name for this target (SQL databases only).
    /// </summary>
    public ScrubTargetBuilder WithSchema(string schema)
    {
        _schema = schema;
        return this;
    }

    /// <summary>
    /// Sets a filter specific to this target (WHERE clause or MongoDB filter).
    /// </summary>
    public ScrubTargetBuilder WithFilter(string filter)
    {
        _filter = filter;
        return this;
    }

    /// <summary>
    /// Adds a field to be masked with the specified rule.
    /// </summary>
    /// <param name="fieldName">The field name to mask.</param>
    /// <param name="ruleId">The ID of the masking rule to apply.</param>
    public ScrubTargetBuilder MaskField(string fieldName, string ruleId)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new ArgumentException("Field name cannot be empty.", nameof(fieldName));

        if (!_ruleRegistry.TryGet(ruleId, out _))
            throw new InvalidOperationException($"Masking rule '{ruleId}' not found in registry.");

        _fields.Add(new FieldMaskingRule
        {
            FieldName = fieldName,
            RuleId = ruleId
        });

        return this;
    }

    /// <summary>
    /// Adds a field to be masked with the specified rule and optional metadata.
    /// </summary>
    public ScrubTargetBuilder MaskField(string fieldName, string ruleId, Dictionary<string, object?> metadata)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            throw new ArgumentException("Field name cannot be empty.", nameof(fieldName));

        if (!_ruleRegistry.TryGet(ruleId, out _))
            throw new InvalidOperationException($"Masking rule '{ruleId}' not found in registry.");

        _fields.Add(new FieldMaskingRule
        {
            FieldName = fieldName,
            RuleId = ruleId,
            Metadata = metadata
        });

        return this;
    }

    /// <summary>
    /// Builds the scrub target.
    /// </summary>
    internal ScrubTarget Build()
    {
        if (_fields.Count == 0)
            throw new InvalidOperationException($"Target '{_tableOrCollection}' must have at least one field to mask.");

        return new ScrubTarget
        {
            Schema = _schema,
            TableOrCollection = _tableOrCollection,
            Fields = _fields.AsReadOnly(),
            Filter = _filter
        };
    }
}
