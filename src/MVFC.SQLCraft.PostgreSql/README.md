# MVFC.SQLCraft.PostgreSql

> 🇧🇷 [Leia em Português](README.pt-br.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft.PostgreSql)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.PostgreSql)

Driver for PostgreSQL access using [MVFC.SQLCraft] and [Npgsql](https://github.com/npgsql/npgsql).

## Installation

```sh
dotnet add package MVFC.SQLCraft.PostgreSql
```

## Features

- Connection with PostgreSQL
- Integration with SqlKata
- Support for .NET 9.0+

## Example

```csharp
using MVFC.SQLCraft.PostgreSql;

PostgreSqlCraftDriver driver = new(connectionString);

driver.Execute("CREATE TABLE IF NOT EXISTS persons (id SERIAL PRIMARY KEY, name VARCHAR(100) NOT NULL);");

var insertQ = new Query("persons")
                    .AsInsert(new 
                    { 
                        name = "Alice" 
                    });

var affected = driver.Execute(insertQ);
```

## License

MIT