# MVFC.SQLCraft.Oracle

> 🇧🇷 [Leia em Português](README.pt-br.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft.Oracle)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Oracle)

Driver for Oracle access using [MVFC.SQLCraft] and [Oracle.ManagedDataAccess.Core](https://www.oracle.com/database/technologies/appdev/dotnet.html).

## Installation

```sh
dotnet add package MVFC.SQLCraft.Oracle
```

## Features

- Connection with Oracle
- Integration with SqlKata
- Support for .NET 9.0+

## Example

```csharp
using MVFC.SQLCraft.Oracle;

OracleSQLCraftDriver driver = new(connectionString);

driver.Execute(@"
            CREATE TABLE ""Persons"" (
                ""Id"" NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
                ""Name"" VARCHAR2(100) NOT NULL
            )");

var insertQ = new Query("Persons").AsInsert(new { Name = "Alice" });
var affected = driver.Execute(insertQ);
```

## License

MIT