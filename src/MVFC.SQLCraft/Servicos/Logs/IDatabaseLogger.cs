namespace MVFC.SQLCraft.Servicos.Logs;

public interface IDatabaseLogger
{
    public Task OnBeforeExecuteAsync(string sql, object? bindings, CancellationToken ct = default);

    public Task OnAfterExecuteAsync(string sql, object? bindings, TimeSpan elapsed, CancellationToken ct = default);

    public Task OnErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct = default);
}
