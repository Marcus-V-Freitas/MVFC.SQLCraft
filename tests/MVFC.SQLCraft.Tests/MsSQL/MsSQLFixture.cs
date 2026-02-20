namespace MVFC.SQLCraft.Tests.MsSQL;

public sealed class SqlServerContainerFixture : IAsyncLifetime
{
    private MsSqlContainer _container = default!;
    public string ConnectionString => _container.GetConnectionString();

    public async ValueTask InitializeAsync()
    {
        _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync() => 
        await _container.DisposeAsync();
}