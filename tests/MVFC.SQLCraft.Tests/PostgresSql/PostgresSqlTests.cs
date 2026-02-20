namespace MVFC.SQLCraft.Tests.PostgresSql;

[Collection("Postgres")]
public sealed class SqlKataHelperPostgresIntegrationTests(PostgresSqlFixture fixture) : IClassFixture<PostgresSqlFixture> {
    private readonly PostgreSqlCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId() {
        _driver.Execute("CREATE TABLE IF NOT EXISTS persons (id SERIAL PRIMARY KEY, name VARCHAR(100) NOT NULL);");

        var insertQ = new Query("persons").AsInsert(new { name = "Alice" });
        var affected = _driver.Execute(insertQ);
        affected.Should().Be(1);

        var selQ = new Query("persons").Select("id", "name").Where("name", "Alice");
        var person = _driver.QueryFirstOrDefault<Person>(selQ);

        person.Should().NotBeNull();
        person!.Name.Should().Be("Alice");
        person.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Testar_Update_E_Select() {
        await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS persons2 (id SERIAL PRIMARY KEY, name VARCHAR(100) NOT NULL);", ct: TestContext.Current.CancellationToken);

        await _driver.ExecuteInTransactionAsync(async (x, t, ct) => {
            await x.ExecuteAsync(new Query("persons2").AsInsert(new { name = "Bob" }), t, ct);

            var sel = new Query("persons2").Select("id", "name").Where("name", "Bob");
            var p = await x.QueryFirstOrDefaultAsync<Person>(sel, t, ct);
            p.Should().NotBeNull();

            var upd = new Query("persons2").Where("id", p!.Id).AsUpdate(new { name = "Robert" });
            var upaff = await x.ExecuteAsync(upd, t, ct);
            upaff.Should().Be(1);

            var sel2 = new Query("persons2").Select("id", "name").Where("id", p.Id);
            var p2 = await x.QueryFirstOrDefaultAsync<Person>(sel2, t, ct);
            p2.Should().NotBeNull();
            p2!.Name.Should().Be("Robert");
        }, ct: TestContext.Current.CancellationToken);
    }
}