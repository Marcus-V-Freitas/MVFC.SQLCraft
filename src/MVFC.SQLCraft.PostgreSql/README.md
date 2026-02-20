# MVFC.SQLCraft.PostgreSql

Driver para acesso ao PostgreSQL usando [MVFC.SQLCraft] e [Npgsql](https://github.com/npgsql/npgsql).

## Instalação

```sh
dotnet add package MVFC.SQLCraft.PostgreSql
```

## Recursos

- Conexão com PostgreSQL
- Integração com SqlKata
- Suporte a .NET 9.0+

## Exemplo

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

## Licença

MIT