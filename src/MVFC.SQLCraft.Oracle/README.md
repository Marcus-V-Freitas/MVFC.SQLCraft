# MVFC.SQLCraft.Oracle

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