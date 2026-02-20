namespace MVFC.SQLCraft.Tests.Oracle;

[Collection("Oracle")]
public sealed class OracleTests(OracleContainerFixture fixture) : IClassFixture<OracleContainerFixture> {
    private readonly OracleSQLCraftDriver _driver = new(fixture.ConnectionString);

    [Fact]
    public void Testar_Insert_E_Select_PorId() {
        _driver.Execute(@"
            CREATE TABLE ""Persons"" (
                ""Id"" NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                ""Name"" VARCHAR2(100) NOT NULL
            )");

        var insertQ = new Query("Persons").AsInsert(new { Name = "Alice" });
        var affected = _driver.Execute(insertQ);
        affected.Should().Be(1);

        var selQ = new Query("Persons").Select("Id", "Name").Where("Name", "Alice");
        var person = _driver.QueryFirstOrDefault<Person>(selQ);

        person.Should().NotBeNull();
        person!.Name.Should().Be("Alice");
        person.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Testar_Update_E_Select() {
        await _driver.ExecuteAsync(@"
            CREATE TABLE ""Persons2"" (
                ""Id"" NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                ""Name"" VARCHAR2(100) NOT NULL
            )", ct: TestContext.Current.CancellationToken);

        await _driver.ExecuteInTransactionAsync(async (x, t, ct) => {
            await x.ExecuteAsync(new Query("Persons2").AsInsert(new { Name = "Bob" }), t, ct);

            var sel = new Query("Persons2").Select("Id", "Name").Where("Name", "Bob");
            var p = await x.QueryFirstOrDefaultAsync<Person>(sel, t, ct);
            p.Should().NotBeNull();

            var upd = new Query("Persons2").Where("Id", p!.Id).AsUpdate(new { Name = "Robert" });
            var upaff = await x.ExecuteAsync(upd, t, ct);
            upaff.Should().Be(1);

            var sel2 = new Query("Persons2").Select("Id", "Name").Where("Id", p.Id);
            var p2 = await x.QueryFirstOrDefaultAsync<Person>(sel2, t, ct);
            p2.Should().NotBeNull();
            p2!.Name.Should().Be("Robert");
        }, ct: TestContext.Current.CancellationToken);
    }
}