namespace MVFC.SQLCraft;

public abstract class SQLCraftDriver(string connectionString, IDatabaseLogger? logger) {
    protected readonly string _connectionString = connectionString;

    protected readonly IDatabaseLogger? _logger = logger;

    protected abstract Compiler Compiler { get; }

    protected abstract DbConnection ConnectionFactory();

    protected virtual IQueryFactory CreateQueryFactory(IDbConnection conn) =>
        new DefaultQueryFactory(conn, Compiler);

    protected virtual (DbConnection? conn, IQueryFactory qf) GetFactory(IDbTransaction? tx) {
        if (tx?.Connection is not null)
            return (null, CreateQueryFactory(tx.Connection));

        var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            conn.Open();

        return (conn, CreateQueryFactory(conn));
    }
    protected virtual async Task<(DbConnection? conn, IQueryFactory qf)> GetFactoryAsync(IDbTransaction? tx, CancellationToken ct) {
        if (tx?.Connection is not null)
            return (null, CreateQueryFactory(tx.Connection));

        var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync(ct);

        return (conn, CreateQueryFactory(conn));
    }

    protected virtual void DisposeConn(DbConnection? conn) =>
        conn?.Dispose();

    protected virtual async Task DisposeConnAsync(DbConnection? conn) {
        if (conn is not null)
            await conn.DisposeAsync();
    }

    protected virtual void LogBefore(string sql, object? bindings) =>
        _logger?.OnBeforeExecuteAsync(sql, bindings).GetAwaiter().GetResult();

    protected virtual void LogAfter(string sql, object? bindings) =>
        _logger?.OnAfterExecuteAsync(sql, bindings, TimeSpan.Zero).GetAwaiter().GetResult();

    protected virtual void LogError(string sql, object? bindings, Exception ex) =>
        _logger?.OnErrorAsync(sql, bindings, ex).GetAwaiter().GetResult();

    protected virtual async Task LogBeforeAsync(string sql, object? bindings, CancellationToken ct) {
        if (_logger != null)
            await _logger.OnBeforeExecuteAsync(sql, bindings, ct);
    }
    protected virtual async Task LogAfterAsync(string sql, object? bindings, CancellationToken ct) {
        if (_logger != null)
            await _logger.OnAfterExecuteAsync(sql, bindings, TimeSpan.Zero, ct);
    }
    protected virtual async Task LogErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct) {
        if (_logger != null)
            await _logger.OnErrorAsync(sql, bindings, ex, ct);
    }

    public virtual T? QueryFirstOrDefault<T>(Query query, IDbTransaction? tx = null) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var result = tx != null ? qf.FirstOrDefault<T>(query, transaction: tx) : qf.FirstOrDefault<T>(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return result;
        }
        catch (Exception ex) {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally {
            DisposeConn(conn);
        }
    }

    public virtual IEnumerable<T> Query<T>(Query query, IDbTransaction? tx = null) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var result = tx != null ? qf.Get<T>(query, transaction: tx) : qf.Get<T>(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return result;
        }
        catch (Exception ex) {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally {
            DisposeConn(conn);
        }
    }

    public virtual async Task<T?> QueryFirstOrDefaultAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct);

        try {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct);

            var result = tx != null ? await qf.FirstOrDefaultAsync<T>(query, transaction: tx, cancellationToken: ct) : await qf.FirstOrDefaultAsync<T>(query, cancellationToken: ct);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct);
            return result;
        }
        catch (Exception ex) {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct);
            throw;
        }
        finally {
            await DisposeConnAsync(conn);
        }
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct);

        try {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct);

            var result = tx != null ? await qf.GetAsync<T>(query, transaction: tx, cancellationToken: ct) : await qf.GetAsync<T>(query, cancellationToken: ct);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct);
            return result;
        }
        catch (Exception ex) {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct);
            throw;
        }
        finally {
            await DisposeConnAsync(conn);
        }
    }

    public virtual int Execute(Query query, IDbTransaction? tx = null) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var affected = tx != null ? qf.Execute(query, transaction: tx) : qf.Execute(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return affected;
        }
        catch (Exception ex) {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally {
            DisposeConn(conn);
        }
    }

    public virtual int Execute(string sql, IDbTransaction? tx = null) {
        var (conn, qf) = GetFactory(tx);

        try {
            LogBefore(sql, null);

            var affected = tx != null ? qf.Statement(sql, transaction: tx) : qf.Statement(sql);

            LogAfter(sql, null);
            return affected;
        }
        catch (Exception ex) {
            LogError(sql, null, ex);
            throw;
        }
        finally {
            DisposeConn(conn);
        }
    }

    public virtual async Task<int> ExecuteAsync(string sql, IDbTransaction? tx = null, CancellationToken ct = default) {
        var (conn, qf) = await GetFactoryAsync(tx, ct);

        try {
            await LogBeforeAsync(sql, null, ct);

            var affected = tx != null ? await qf.StatementAsync(sql, transaction: tx, cancellationToken: ct) : await qf.StatementAsync(sql, cancellationToken: ct);

            await LogAfterAsync(sql, null, ct);
            return affected;
        }
        catch (Exception ex) {
            await LogErrorAsync(sql, null, ex, ct);
            throw;
        }
        finally {
            await DisposeConnAsync(conn);
        }
    }

    public virtual async Task<int> ExecuteAsync(Query query, IDbTransaction? tx = null, CancellationToken ct = default) {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct);

        try {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct);

            var affected = tx != null ? await qf.ExecuteAsync(query, transaction: tx, cancellationToken: ct) : await qf.ExecuteAsync(query, cancellationToken: ct);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct);
            return affected;
        }
        catch (Exception ex) {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct);
            throw;
        }
        finally {
            await DisposeConnAsync(conn);
        }
    }

    public virtual void ExecuteInTransaction(Action<SQLCraftDriver, IDbTransaction> action, IsolationLevel isolation = IsolationLevel.ReadCommitted) {
        using var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            conn.Open();

        using var tx = conn.BeginTransaction(isolation);

        try {
            action(this, tx);
            tx.Commit();
        }
        catch {
            tx.Rollback();
            throw;
        }
    }

    public virtual async Task ExecuteInTransactionAsync(Func<SQLCraftDriver, IDbTransaction, CancellationToken, Task> action, IsolationLevel isolation = IsolationLevel.ReadCommitted, CancellationToken ct = default) {
        await using var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync(ct);

        await using var tx = await conn.BeginTransactionAsync(isolation, ct);

        try {
            await action(this, tx, ct);
            await tx.CommitAsync(ct);
        }
        catch {
            await tx.RollbackAsync(ct);
            throw;
        }
    }
}