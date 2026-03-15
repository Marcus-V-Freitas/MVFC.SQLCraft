# MVFC.SQLCraft.Mysql

> 🇺🇸 [Read in English](README.md)

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
