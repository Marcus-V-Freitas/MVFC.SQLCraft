namespace MVFC.SQLCraft.Servicos.Logs;

public interface IDatabaseLogger {
    Task OnBeforeExecuteAsync(string sql, object? bindings, CancellationToken ct = default);
    Task OnAfterExecuteAsync(string sql, object? bindings, TimeSpan elapsed, CancellationToken ct = default);
    Task OnErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct = default);
}