# MVFC.SQLCraft.SQLite

> 🇺🇸 [Read in English](README.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft.SQLite)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.SQLite)

Driver para acesso ao SQLite usando [MVFC.SQLCraft] e [System.Data.SQLite](https://system.data.sqlite.org/home/doc/trunk/www/index.md).

## Instalação

```sh
dotnet add package MVFC.SQLCraft.SQLite
```

## Recursos

- Conexão com SQLite
- Integração com SqlKata
- Suporte a .NET 9.0+

## Exemplo

```csharp
using MVFC.SQLCraft.SQLite;

SQLiteCraftDriver driver = new(connectionString);

driver.Execute("CREATE TABLE IF NOT EXISTS Persons (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL);");
var insertQ = new Query("Persons")
                    .AsInsert(new 
                    {
                         Name = "Alice" 
                    });

var affected = driver.Execute(insertQ);

```

## Licença

MIT
