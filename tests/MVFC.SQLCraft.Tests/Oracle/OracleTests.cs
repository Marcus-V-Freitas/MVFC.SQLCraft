namespace MVFC.SQLCraft.Tests.Oracle;

[Collection("Oracle")]
public sealed class OracleTests(OracleContainerFixture fixture) : IClassFixture<OracleContainerFixture>
{
    private readonly OracleSQLCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId()
    {
        _driver.Execute(@"
            CREATE TABLE ""Persons"" (
                ""Id"" NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                ""Name"" VARCHAR2(100) NOT NULL
            )");

        TestHelpers.ExecuteOperations(_driver);
    }

    [Fact]
    public async Task Testar_Update_E_Select()
    {
        await _driver.ExecuteAsync(@"
            CREATE TABLE ""Persons2"" (
                ""Id"" NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                ""Name"" VARCHAR2(100) NOT NULL
            )", ct: TestContext.Current.CancellationToken);

        await TestHelpers.ExecuteOperationsAsync(_driver, TestContext.Current.CancellationToken);
    }
}
