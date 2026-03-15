# MVFC.SQLCraft.Oracle

> 🇺🇸 [Read in English](README.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft.Oracle)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Oracle)

Driver para acesso ao Oracle usando o [MVFC.SQLCraft] e [Oracle.ManagedDataAccess.Core](https://www.oracle.com/database/technologies/appdev/dotnet.html).

## Instalação

```sh
dotnet add package MVFC.SQLCraft.Oracle
```

## Recursos

- Conexão com Oracle
- Integração com SqlKata
- Suporte a .NET 9.0+

## Exemplo

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

## Licença

MIT
