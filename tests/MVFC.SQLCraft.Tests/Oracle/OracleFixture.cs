namespace MVFC.SQLCraft.Tests.Oracle;

public sealed class OracleContainerFixture : IAsyncLifetime
{
    private OracleContainer _container = default!;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync() {
        _container = new OracleBuilder("gvenzl/oracle-xe:21-slim")
            .WithPassword("MySecretPassword123")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync().ConfigureAwait(true);
    }

    public async ValueTask DisposeAsync() => 
        await _container.DisposeAsync().ConfigureAwait(true);
}
