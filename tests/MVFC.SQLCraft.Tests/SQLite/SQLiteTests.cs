namespace MVFC.SQLCraft.Tests.SQLite;

[Collection("SQLite")]

public sealed class SQLiteTests(SQLiteFixture fixture) : IClassFixture<SQLiteFixture>
{
    private readonly SQLiteCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId()
    {
        _driver.Execute("CREATE TABLE IF NOT EXISTS Persons (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);");

        TestHelpers.ExecuteOperations(_driver);
    }

    [Fact]
    public async Task Testar_Update_E_Select()
    {
        await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS Persons2 (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);", ct: TestContext.Current.CancellationToken);

        await TestHelpers.ExecuteOperationsAsync(_driver, TestContext.Current.CancellationToken);
    }
}
