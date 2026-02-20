namespace MVFC.SQLCraft.Tests.Firebird;

public sealed class FirebirdContainerFixture : IAsyncLifetime
{
    private FirebirdSqlContainer _container = default!;

    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        _container = new FirebirdSqlBuilder("jacobalberty/firebird:latest")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync() => 
        await _container.DisposeAsync();
}