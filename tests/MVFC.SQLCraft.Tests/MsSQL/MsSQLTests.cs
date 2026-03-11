namespace MVFC.SQLCraft.Tests.MsSQL;

[Collection("SqlServer")]
public class SqlKataHelperSqlServerIntegrationTests(SqlServerContainerFixture fixture) : IClassFixture<SqlServerContainerFixture>
{
    private readonly MsSQLCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId()
    {
        _driver.Execute("IF OBJECT_ID('dbo.Persons', 'U') IS NULL CREATE TABLE Persons (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(100) NOT NULL);");

        TestHelpers.ExecuteOperations(_driver);
    }

    [Fact]
    public async Task Testar_Update_E_Select()
    {
        await _driver.ExecuteAsync("IF OBJECT_ID('dbo.Persons2', 'U') IS NULL CREATE TABLE Persons2 (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(100) NOT NULL);", ct: TestContext.Current.CancellationToken);

        await TestHelpers.ExecuteOperationsAsync(_driver, TestContext.Current.CancellationToken);
    }
}
