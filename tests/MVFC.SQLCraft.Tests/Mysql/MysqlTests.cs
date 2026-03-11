namespace MVFC.SQLCraft.Tests.Mysql;

[Collection("MySql")]
public sealed class SqlKataHelperIntegrationTests(MySqlContainerFixture fixture) : IClassFixture<MySqlContainerFixture>
{
    private readonly MysqlCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId()
    {
        _driver.Execute("CREATE TABLE Persons (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(100) NOT NULL);");

        TestHelpers.ExecuteOperations(_driver);
    }

    [Fact]
    public async Task Testar_Update_E_Select()
    {
        await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS Persons2 (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(100) NOT NULL);", ct: TestContext.Current.CancellationToken);

        await TestHelpers.ExecuteOperationsAsync(_driver, TestContext.Current.CancellationToken);
    }
}
