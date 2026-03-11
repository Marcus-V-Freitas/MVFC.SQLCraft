namespace MVFC.SQLCraft.Tests.SQLite;

public sealed class SQLiteFixture : IAsyncLifetime
{
    private string _databasePath = default!;

    public string ConnectionString { get; private set; } = default!;

    public async ValueTask InitializeAsync()
    {
        _databasePath = Path.GetTempFileName() + ".sqlite";
        ConnectionString = $"Data Source={_databasePath};Version=3;";

        await Task.CompletedTask.ConfigureAwait(true);
    }

    public async ValueTask DisposeAsync()
    {
        if (File.Exists(_databasePath))
            File.Delete(_databasePath);

        await Task.CompletedTask.ConfigureAwait(true);
    }
}
