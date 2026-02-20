namespace MVFC.SQLCraft.Tests.CustomDriver.CustomUtils;

public sealed class CustomTestQueryFactory(IDbConnection conn, Compiler compiler, CustomTestCraftDriver driver) : IQueryFactory {
    private readonly CustomTestCraftDriver _driver = driver;
    private readonly QueryFactory _queryFactory = new(conn, compiler);

    public T? FirstOrDefault<T>(Query query, IDbTransaction? transaction = null, int? timeout = null) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in FirstOrDefault")
            : _queryFactory.FirstOrDefault<T>(query, transaction, timeout);

    public IEnumerable<T> Get<T>(Query query, IDbTransaction? transaction = null, int? timeout = null) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in Get")
            : _queryFactory.Get<T>(query, transaction, timeout);

    public int Execute(Query query, IDbTransaction? transaction = null, int? timeout = null) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in Execute")
            : _queryFactory.Execute(query, transaction, timeout);

    public async Task<T?> FirstOrDefaultAsync<T>(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in FirstOrDefaultAsync")
            : await _queryFactory.FirstOrDefaultAsync<T>(query, transaction, timeout, cancellationToken);

    public async Task<IEnumerable<T>> GetAsync<T>(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in GetAsync")
            : await _queryFactory.GetAsync<T>(query, transaction, timeout, cancellationToken);

    public async Task<int> ExecuteAsync(Query query, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in ExecuteAsync")
            : await _queryFactory.ExecuteAsync(query, transaction, timeout, cancellationToken);

    public int Statement(string sql, object? param = null, IDbTransaction? transaction = null, int? timeout = null) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in Statement")
            : _queryFactory.Statement(sql, param, transaction, timeout);

    public async Task<int> StatementAsync(string sql, object? param = null, IDbTransaction? transaction = null, int? timeout = null, CancellationToken cancellationToken = default) => _driver.ThrowInternalError
            ? throw new InvalidOperationException("Simulated exception in StatementAsync")
            : await _queryFactory.StatementAsync(sql, param, transaction, timeout, cancellationToken);
}