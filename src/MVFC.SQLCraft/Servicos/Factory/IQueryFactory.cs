namespace MVFC.SQLCraft.Servicos.Factory;

public interface IQueryFactory {
    public T? FirstOrDefault<T>(Query query, IDbTransaction? transaction = null, int? timeout = null);

    public IEnumerable<T> Get<T>(Query query, IDbTransaction? transaction = null, int? timeout = null);

    public int Execute(Query query, IDbTransaction? transaction = null, int? timeout = null);

    public Task<T?> FirstOrDefaultAsync<T>(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default);

    public Task<IEnumerable<T>> GetAsync<T>(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default);

    public Task<int> ExecuteAsync(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default);

    public int Statement(string sql, object? param = null, IDbTransaction? transaction = null, int? timeout = null);

    public Task<int> StatementAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default);
}