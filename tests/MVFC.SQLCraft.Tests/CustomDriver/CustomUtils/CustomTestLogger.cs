namespace MVFC.SQLCraft.Tests.CustomDriver.CustomUtils;

public sealed class CustomTestLogger : IDatabaseLogger {
    public bool BeforeCalled { get; private set; }
    public bool AfterCalled { get; private set; }
    public bool ErrorCalled { get; private set; }
    public Exception? LastException { get; private set; }
    public string? LastSql { get; private set; }

    public Task OnBeforeExecuteAsync(string sql, object? bindings, CancellationToken ct = default) {
        BeforeCalled = true;
        LastSql = sql;
        return Task.CompletedTask;
    }

    public Task OnAfterExecuteAsync(string sql, object? bindings, TimeSpan elapsed, CancellationToken ct = default) {
        AfterCalled = true;
        LastSql = sql;
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct = default) {
        ErrorCalled = true;
        LastException = ex;
        LastSql = sql;
        return Task.CompletedTask;
    }
}