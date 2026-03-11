namespace MVFC.SQLCraft.Tests.PostgresSql;

[Collection("Postgres")]
public sealed class SqlKataHelperPostgresIntegrationTests(PostgresSqlFixture fixture) : IClassFixture<PostgresSqlFixture>
{
    private readonly PostgreSqlCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId()
    {
        _driver.Execute(@"
            CREATE TABLE IF NOT EXISTS ""Persons"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""Name"" VARCHAR(100) NOT NULL
            )");

        TestHelpers.ExecuteOperations(_driver);
    }

    [Fact]
    public async Task Testar_Update_E_Select()
    {
        await _driver.ExecuteAsync(@"
            CREATE TABLE IF NOT EXISTS ""Persons2"" (
                ""Id"" SERIAL PRIMARY KEY,
                ""Name"" VARCHAR(100) NOT NULL
            )", ct: TestContext.Current.CancellationToken);

        await TestHelpers.ExecuteOperationsAsync(_driver, TestContext.Current.CancellationToken);
    }
}
