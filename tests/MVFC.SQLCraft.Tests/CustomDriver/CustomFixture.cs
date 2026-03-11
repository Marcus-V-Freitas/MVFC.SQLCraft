namespace MVFC.SQLCraft.Tests.CustomDriver;

public sealed class CustomFixture : IAsyncLifetime
{
    private PostgreSqlContainer _container = default!;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() {
        _container = new PostgreSqlBuilder("postgres:15.1")
                                .WithCleanUp(true)
                                .Build();

        await _container.StartAsync().ConfigureAwait(true);
    }

    public async ValueTask DisposeAsync() =>
        await _container.DisposeAsync().ConfigureAwait(true);
}
