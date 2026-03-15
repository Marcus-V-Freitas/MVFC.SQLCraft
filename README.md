# MVFC.SQLCraft

> 🇧🇷 [Leia em Português](README.pt-br.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft)

A complete .NET library suite for SQL database abstraction and fluent query building, with multi-database support powered by [SqlKata](https://github.com/sqlkata/querybuilder).

## Motivation

Working with multiple SQL databases in .NET often means:

- Writing repetitive boilerplate for connection management and disposal.
- Manually wiring up different ADO.NET drivers per database.
- Duplicating transaction handling logic across repositories.
- Losing observability without a consistent logging hook.
- Mixing raw SQL strings with query builder calls inconsistently.

**MVFC.SQLCraft** solves this by providing a single abstract driver base that every database-specific package extends. You pick the driver for your database, inherit from it, and get a consistent, battle-tested API for querying, executing, and managing transactions — with optional structured logging built in.

## Architecture

All packages follow the same pattern:

- `SQLCraftDriver` (base class) — manages connections, logging lifecycle, and transaction scope.
- `XxxCraftDriver` (driver class per database) — provides the `DbConnection` factory and the SqlKata `Compiler` for its dialect.
- `IDatabaseLogger` (optional interface) — intercepts SQL before/after execution and on errors.
- `IQueryFactory` / `DefaultQueryFactory` — thin wrapper around SqlKata's `QueryFactory`.

Once you understand one driver, all others work identically.

## Packages

| Package | Database | Downloads |
|---|---|---|
| [MVFC.SQLCraft](src/MVFC.SQLCraft/README.md) | Base abstractions | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft) |
| [MVFC.SQLCraft.Mysql](src/MVFC.SQLCraft.Mysql/README.md) | MySQL / MariaDB | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Mysql) |
| [MVFC.SQLCraft.MsSQL](src/MVFC.SQLCraft.MsSQL/README.md) | Microsoft SQL Server | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.MsSQL) |
| [MVFC.SQLCraft.PostgreSql](src/MVFC.SQLCraft.PostgreSql/README.md) | PostgreSQL | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.PostgreSql) |
| [MVFC.SQLCraft.SQLite](src/MVFC.SQLCraft.SQLite/README.md) | SQLite | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.SQLite) |
| [MVFC.SQLCraft.Firebird](src/MVFC.SQLCraft.Firebird/README.md) | Firebird | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Firebird) |
| [MVFC.SQLCraft.Oracle](src/MVFC.SQLCraft.Oracle/README.md) | Oracle | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Oracle) |

***

## Installation

Install the base package and the driver for your target database:

```sh
# Base abstractions (required)
dotnet add package MVFC.SQLCraft

# Pick your driver
dotnet add package MVFC.SQLCraft.Mysql
dotnet add package MVFC.SQLCraft.MsSQL
dotnet add package MVFC.SQLCraft.PostgreSql
dotnet add package MVFC.SQLCraft.SQLite
dotnet add package MVFC.SQLCraft.Firebird
dotnet add package MVFC.SQLCraft.Oracle
```

## Quick Start

### 1. Create your driver
Inherit from the database-specific driver and pass your connection string:

```csharp
using MVFC.SQLCraft.Mysql;

public class MyAppDatabase : MysqlCraftDriver
{
    public MyAppDatabase(string connectionString)
        : base(connectionString, logger: null) { }
}
```

### 2. Register with DI

```csharp
builder.Services.AddSingleton<MyAppDatabase>(_ =>
    new MyAppDatabase(builder.Configuration.GetConnectionString("Default")!));
```

### 3. Query fluently

```csharp
public class UserRepository
{
    private readonly MyAppDatabase _db;

    public UserRepository(MyAppDatabase db) => _db = db;

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken ct = default)
    {
        var query = new Query("users")
            .Where("active", true)
            .OrderByDesc("created_at")
            .Limit(100);

        return await _db.QueryAsync<User>(query, ct: ct);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var query = new Query("users").Where("id", id);
        return await _db.QueryFirstOrDefaultAsync<User>(query, ct: ct);
    }
}
```

## Available API
All methods are defined on SQLCraftDriver and available in every driver.

### Query Methods

| Method | Description |
|---|---|
| Query<T>(query, tx?) | Returns all matching rows |
| QueryAsync<T>(query, tx?, ct) | Async version of Query<T> |
| QueryFirstOrDefault<T>(query, tx?) | Returns the first matching row or null |
| QueryFirstOrDefaultAsync<T>(query, tx?, ct) | Async version |

### Execute Methods

| Method | Description |
|---|---|
| Execute(query, tx?) | Executes a SqlKata Query (INSERT/UPDATE/DELETE) |
| Execute(sql, tx?) | Executes a raw SQL string |
| ExecuteAsync(query, tx?, ct) | Async version for Query |
| ExecuteAsync(sql, tx?, ct) | Async version for raw SQL |

### Transaction Methods

| Method | Description |
|---|---|
| ExecuteInTransaction(action, isolation?) | Wraps a synchronous action in a managed transaction |
| ExecuteInTransactionAsync(action, isolation?, ct) | Async version — commits on success, rolls back on exception |

## Transactions
The transaction wrappers manage the full lifecycle (open, commit, rollback, dispose) automatically:

```csharp
await _db.ExecuteInTransactionAsync(async (driver, tx, ct) =>
{
    var insertOrder = new Query("orders").AsInsert(new { CustomerId = 42, Total = 199.90m });
    await driver.ExecuteAsync(insertOrder, tx, ct);

    var updateStock = new Query("products").Where("id", 7).AsDecrement("stock", 1);
    await driver.ExecuteAsync(updateStock, tx, ct);
}, isolation: IsolationLevel.ReadCommitted, ct: cancellationToken);
```

If any operation throws, the transaction is automatically rolled back.

## Structured Logging
Implement IDatabaseLogger to intercept all SQL executions:

```csharp
public class MyDatabaseLogger : IDatabaseLogger
{
    private readonly ILogger<MyDatabaseLogger> _logger;

    public MyDatabaseLogger(ILogger<MyDatabaseLogger> logger) => _logger = logger;

    public Task OnBeforeExecuteAsync(string sql, object? bindings, CancellationToken ct = default)
    {
        _logger.LogDebug("Executing SQL: {Sql} | Bindings: {Bindings}", sql, bindings);
        return Task.CompletedTask;
    }

    public Task OnAfterExecuteAsync(string sql, object? bindings, TimeSpan elapsed, CancellationToken ct = default)
    {
        _logger.LogInformation("SQL executed in {Elapsed}ms: {Sql}", elapsed.TotalMilliseconds, sql);
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct = default)
    {
        _logger.LogError(ex, "SQL failed: {Sql} | Bindings: {Bindings}", sql, bindings);
        return Task.CompletedTask;
    }
}
```

Then wire it up:

```csharp
public class MyAppDatabase : MysqlCraftDriver
{
    public MyAppDatabase(string connectionString, MyDatabaseLogger logger)
        : base(connectionString, logger) { }
}
```

## Project Structure

```text
src/
  MVFC.SQLCraft/               # Base abstractions (SQLCraftDriver, IDatabaseLogger, IQueryFactory)
  MVFC.SQLCraft.Firebird/      # Firebird driver
  MVFC.SQLCraft.MsSQL/         # SQL Server driver
  MVFC.SQLCraft.Mysql/         # MySQL / MariaDB driver
  MVFC.SQLCraft.Oracle/        # Oracle driver
  MVFC.SQLCraft.PostgreSql/    # PostgreSQL driver
  MVFC.SQLCraft.SQLite/        # SQLite driver
tests/
  # Unit and integration tests
tools/
  # Build tooling (Cake)
```

## Requirements
.NET 9.0+

SqlKata >= 2.x

The underlying ADO.NET provider for each database (pulled automatically via NuGet)

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md).

## License

[Apache-2.0](LICENSE)
