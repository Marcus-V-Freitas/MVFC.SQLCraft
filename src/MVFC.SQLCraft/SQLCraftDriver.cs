namespace MVFC.SQLCraft;

public abstract class SQLCraftDriver(string connectionString, IDatabaseLogger? logger)
{
    protected readonly string _connectionString = connectionString;

    protected readonly IDatabaseLogger? _logger = logger;

    protected abstract Compiler Compiler { get; }

    protected abstract DbConnection ConnectionFactory();

    protected virtual IQueryFactory CreateQueryFactory(IDbConnection conn) =>
        new DefaultQueryFactory(conn, Compiler);

    protected virtual (DbConnection? conn, IQueryFactory qf) GetFactory(IDbTransaction? tx)
    {
        if (tx?.Connection is not null)
            return (null, CreateQueryFactory(tx.Connection));

        var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            conn.Open();

        return (conn, CreateQueryFactory(conn));
    }

    protected virtual async Task<(DbConnection? conn, IQueryFactory qf)> GetFactoryAsync(IDbTransaction? tx, CancellationToken ct)
    {
        if (tx?.Connection is not null)
            return (null, CreateQueryFactory(tx.Connection));

        var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync(ct).ConfigureAwait(false);

        return (conn, CreateQueryFactory(conn));
    }

    protected virtual void DisposeConn(DbConnection? conn) =>
        conn?.Dispose();

    protected virtual async Task DisposeConnAsync(DbConnection? conn) {
        if (conn is not null)
            await conn.DisposeAsync().ConfigureAwait(false);
    }

    protected virtual void LogBefore(string sql, object? bindings) =>
        _logger?.OnBeforeExecuteAsync(sql, bindings).GetAwaiter().GetResult();

    protected virtual void LogAfter(string sql, object? bindings) =>
        _logger?.OnAfterExecuteAsync(sql, bindings, TimeSpan.Zero).GetAwaiter().GetResult();

    protected virtual void LogError(string sql, object? bindings, Exception ex) =>
        _logger?.OnErrorAsync(sql, bindings, ex).GetAwaiter().GetResult();

    protected virtual async Task LogBeforeAsync(string sql, object? bindings, CancellationToken ct)
    {
        if (_logger != null)
            await _logger.OnBeforeExecuteAsync(sql, bindings, ct).ConfigureAwait(false);
    }

    protected virtual async Task LogAfterAsync(string sql, object? bindings, CancellationToken ct)
    {
        if (_logger != null)
            await _logger.OnAfterExecuteAsync(sql, bindings, TimeSpan.Zero, ct).ConfigureAwait(false);
    }

    protected virtual async Task LogErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct)
    {
        if (_logger != null)
            await _logger.OnErrorAsync(sql, bindings, ex, ct).ConfigureAwait(false);
    }

    public virtual T? QueryFirstOrDefault<T>(Query query, IDbTransaction? tx = null)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try
        {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var result = tx != null ? qf.FirstOrDefault<T>(query, transaction: tx) : qf.FirstOrDefault<T>(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return result;
        }
        catch (Exception ex)
        {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally
        {
            DisposeConn(conn);
        }
    }

    public virtual IEnumerable<T> Query<T>(Query query, IDbTransaction? tx = null)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try
        {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var result = tx != null ? qf.Get<T>(query, transaction: tx) : qf.Get<T>(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return result;
        }
        catch (Exception ex)
        {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally
        {
            DisposeConn(conn);
        }
    }

    public virtual async Task<T?> QueryFirstOrDefaultAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct).ConfigureAwait(false);

        try
        {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);

            var result = tx != null ?
                await qf.FirstOrDefaultAsync<T>(query, transaction: tx, cancellationToken: ct).ConfigureAwait(false) :
                await qf.FirstOrDefaultAsync<T>(query, cancellationToken: ct).ConfigureAwait(false);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct).ConfigureAwait(false);
            throw;
        }
        finally
        {
            await DisposeConnAsync(conn).ConfigureAwait(false);
        }
    }

    public virtual async Task<IEnumerable<T>> QueryAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct).ConfigureAwait(false);

        try
        {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);

            var result = tx != null ?
                await qf.GetAsync<T>(query, transaction: tx, cancellationToken: ct).ConfigureAwait(false) :
                await qf.GetAsync<T>(query, cancellationToken: ct).ConfigureAwait(false);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct).ConfigureAwait(false);
            throw;
        }
        finally
        {
            await DisposeConnAsync(conn).ConfigureAwait(false);
        }
    }

    public virtual int Execute(Query query, IDbTransaction? tx = null)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = GetFactory(tx);

        try
        {
            LogBefore(compiled.Sql, compiled.NamedBindings);

            var affected = tx != null ? qf.Execute(query, transaction: tx) : qf.Execute(query);

            LogAfter(compiled.Sql, compiled.NamedBindings);
            return affected;
        }
        catch (Exception ex)
        {
            LogError(compiled.Sql, compiled.NamedBindings, ex);
            throw;
        }
        finally
        {
            DisposeConn(conn);
        }
    }

    public virtual int Execute(string sql, IDbTransaction? tx = null)
    {
        var (conn, qf) = GetFactory(tx);

        try
        {
            LogBefore(sql, null);

            var affected = tx != null ? qf.Statement(sql, transaction: tx) : qf.Statement(sql);

            LogAfter(sql, null);
            return affected;
        }
        catch (Exception ex)
        {
            LogError(sql, null, ex);
            throw;
        }
        finally {
            DisposeConn(conn);
        }
    }

    public virtual async Task<int> ExecuteAsync(string sql, IDbTransaction? tx = null, CancellationToken ct = default)
    {
        var (conn, qf) = await GetFactoryAsync(tx, ct).ConfigureAwait(false);

        try
        {
            await LogBeforeAsync(sql, null, ct).ConfigureAwait(false);

            var affected = tx != null ?
                await qf.StatementAsync(sql, transaction: tx, cancellationToken: ct).ConfigureAwait(false) :
                await qf.StatementAsync(sql, cancellationToken: ct).ConfigureAwait(false);

            await LogAfterAsync(sql, null, ct).ConfigureAwait(false);
            return affected;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(sql, null, ex, ct).ConfigureAwait(false);
            throw;
        }
        finally
        {
            await DisposeConnAsync(conn).ConfigureAwait(false);
        }
    }

    public virtual async Task<int> ExecuteAsync(Query query, IDbTransaction? tx = null, CancellationToken ct = default)
    {
        var compiled = Compiler.Compile(query);
        var (conn, qf) = await GetFactoryAsync(tx, ct).ConfigureAwait(false);

        try
        {
            await LogBeforeAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);

            var affected = tx != null ?
                await qf.ExecuteAsync(query, transaction: tx, cancellationToken: ct).ConfigureAwait(false) :
                await qf.ExecuteAsync(query, cancellationToken: ct).ConfigureAwait(false);

            await LogAfterAsync(compiled.Sql, compiled.NamedBindings, ct).ConfigureAwait(false);
            return affected;
        }
        catch (Exception ex)
        {
            await LogErrorAsync(compiled.Sql, compiled.NamedBindings, ex, ct).ConfigureAwait(false);
            throw;
        }
        finally
        {
            await DisposeConnAsync(conn).ConfigureAwait(false);
        }
    }

    public virtual void ExecuteInTransaction(Action<SQLCraftDriver, IDbTransaction> action, IsolationLevel isolation = IsolationLevel.ReadCommitted)
    {
        ArgumentNullException.ThrowIfNull(action);

        using var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            conn.Open();

        using var tx = conn.BeginTransaction(isolation);

        try
        {
            action(this, tx);
            tx.Commit();
        }
        catch
        {
            tx.Rollback();
            throw;
        }
    }

    public virtual async Task ExecuteInTransactionAsync(Func<SQLCraftDriver, IDbTransaction, CancellationToken, Task> action, IsolationLevel isolation = IsolationLevel.ReadCommitted, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(action);

        await using var conn = ConnectionFactory();

        if (conn.State != ConnectionState.Open)
            await conn.OpenAsync(ct).ConfigureAwait(false);

        await using var tx = await conn.BeginTransactionAsync(isolation, ct).ConfigureAwait(false);

        try
        {
            await action(this, tx, ct).ConfigureAwait(false);
            await tx.CommitAsync(ct).ConfigureAwait(false);
        }
        catch
        {
            await tx.RollbackAsync(ct).ConfigureAwait(false);
            throw;
        }
    }
}
