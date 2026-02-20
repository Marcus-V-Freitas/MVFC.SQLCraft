namespace MVFC.SQLCraft.Tests.CustomDriver.CustomUtils;

public sealed class CustomTestCraftDriver(string conn, IDatabaseLogger? logger = null) : SQLCraftDriver(conn, logger) {
    public bool DisposeConnCalled { get; private set; }
    public bool DisposeConnAsyncCalled { get; private set; }
    public bool LogBeforeCalled { get; private set; }
    public bool LogAfterCalled { get; private set; }
    public bool LogErrorCalled { get; private set; }

    public bool ThrowInternalError { get; set; }

    protected override Compiler Compiler => new PostgresCompiler();

    protected override IQueryFactory CreateQueryFactory(IDbConnection conn) =>
        new CustomTestQueryFactory(conn, Compiler, this);

    protected override DbConnection ConnectionFactory() => new NpgsqlConnection(_connectionString);

    protected override void DisposeConn(DbConnection? conn) {
        DisposeConnCalled = true;
        base.DisposeConn(conn);
    }

    protected override async Task DisposeConnAsync(DbConnection? conn) {
        DisposeConnAsyncCalled = true;
        await base.DisposeConnAsync(conn);
    }

    protected override void LogBefore(string sql, object? bindings) {
        LogBeforeCalled = true;
        base.LogBefore(sql, bindings);
    }

    protected override void LogAfter(string sql, object? bindings) {
        LogAfterCalled = true;
        base.LogAfter(sql, bindings);
    }

    protected override void LogError(string sql, object? bindings, Exception ex) {
        LogErrorCalled = true;
        base.LogError(sql, bindings, ex);
    }
}