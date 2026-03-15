# MVFC.SQLCraft.SQLite

> 🇺🇸 [Read in English](README.md)

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
