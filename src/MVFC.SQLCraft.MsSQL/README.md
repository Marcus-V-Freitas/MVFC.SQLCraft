# MVFC.SQLCraft.MsSQL

Driver para acesso ao Microsoft SQL Server usando [MVFC.SQLCraft] e [Microsoft.Data.SqlClient](https://github.com/dotnet/SqlClient).

## Instalação

```sh
dotnet add package MVFC.SQLCraft.MsSQL
```

## Recursos

- Conexão segura com SQL Server
- Integração com SqlKata
- Suporte a .NET 9.0+

## Exemplo

```csharp
using MVFC.SQLCraft.MsSQL;

MsSQLCraftDriver _driver = new(connectionString);

driver.Execute("IF OBJECT_ID('dbo.Persons', 'U') IS NULL CREATE TABLE Persons (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(100) NOT NULL);");

var insertQ = new Query("Persons")
                    .AsInsert(new 
                    { 
                        Name = "Alice" 
                    });

var affected = driver.Execute(insertQ);
```

## Licença

MIT