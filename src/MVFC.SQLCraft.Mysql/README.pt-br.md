# MVFC.SQLCraft.Mysql

> 🇺🇸 [Read in English](README.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft.Mysql)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Mysql)

Driver para acesso ao MySQL/MariaDB usando [MVFC.SQLCraft] e [MySqlConnector](https://github.com/mysql-net/MySqlConnector).

## Instalação

```sh
dotnet add package MVFC.SQLCraft.Mysql
```

## Recursos

- Conexão simplificada com MySQL/MariaDB
- Compatível com .NET 9.0+
- Integração com SqlKata

## Exemplo

```csharp
using MVFC.SQLCraft.Mysql;

MysqlCraftDriver driver = new(connectionString);

driver.Execute("CREATE TABLE Persons (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(100) NOT NULL);");

var insertQ = new Query("Persons")
                    .AsInsert(new 
                    {
                         Name = "Alice" 
                    });
                    
var affected = driver.Execute(insertQ);
```

## Licença

MIT
