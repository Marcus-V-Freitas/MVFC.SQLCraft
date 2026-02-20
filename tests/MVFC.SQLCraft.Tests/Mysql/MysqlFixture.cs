namespace MVFC.SQLCraft.Tests.Mysql;

public sealed class MySqlContainerFixture : IAsyncLifetime
{
    private MySqlContainer _container = default!;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        _container = new MySqlBuilder("mysql:8.0")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync() => 
        await _container.DisposeAsync();
}