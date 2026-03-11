namespace MVFC.SQLCraft.Tests.Helpers;

public static class TestHelpers
{
    public static void ExecuteOperations(SQLCraftDriver driver)
    {
        ArgumentNullException.ThrowIfNull(driver);

        var insertQ = InsertPersonQuery("Persons", "Alice");
        var affected = driver.Execute(insertQ);
        affected.Should().Be(1);

        var selQ = SelectPersonQuery("Persons", "Name", "Alice");
        var person = driver.QueryFirstOrDefault<Person>(selQ);

        person.Should().NotBeNull();
        person!.Name.Should().Be("Alice");
        person.Id.Should().BeGreaterThan(0);
    }

    public static async Task ExecuteOperationsAsync(SQLCraftDriver driver, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(driver);

        await driver.ExecuteInTransactionAsync(ExecuteTransactionAsync, ct: ct)
                    .ConfigureAwait(false);
    }

    private static async Task ExecuteTransactionAsync(SQLCraftDriver driver, IDbTransaction transaction, CancellationToken ct)
    {
        var insertQ = InsertPersonQuery("Persons2", "Bob");
        await driver.ExecuteAsync(insertQ, transaction, ct).ConfigureAwait(true);

        var selectBobQ = SelectPersonQuery("Persons2", "Name", "Bob");
        var person = await driver.QueryFirstOrDefaultAsync<Person>(selectBobQ, transaction, ct).ConfigureAwait(true);
        person.Should().NotBeNull();

        var updateQ = UpdatePersonQuery("Persons2", person!.Id, "Robert");
        var updated = await driver.ExecuteAsync(updateQ, transaction, ct).ConfigureAwait(true);
        updated.Should().Be(1);

        var selectIdQ = SelectPersonQuery("Persons2", "Id", person!.Id);
        person = await driver.QueryFirstOrDefaultAsync<Person>(selectIdQ, transaction, ct).ConfigureAwait(true);
        person.Should().NotBeNull();
        person!.Name.Should().Be("Robert");
    }

    private static Query InsertPersonQuery(string tableName, string name) =>
        new Query(tableName)
              .AsInsert(new
              {
                  Name = name,
              });

    private static Query SelectPersonQuery(string tableName, string name, object value) =>
        new Query(tableName)
              .Select("Id", "Name")
              .Where(name, value);

    private static Query UpdatePersonQuery(string tableName, int id, string name) =>
        new Query(tableName)
              .Where("Id", id)
              .AsUpdate(new
              {
                  Name = name,
              });
}
