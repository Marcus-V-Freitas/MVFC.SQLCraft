namespace MVFC.SQLCraft.Tests.PostgresSql;

public sealed class PostgresSqlFixture : IAsyncLifetime
{
    private PostgreSqlContainer _container = default!;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        _container = new PostgreSqlBuilder("postgres:15.1")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync() => 
        await _container.DisposeAsync();
}