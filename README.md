# MVFC.SQLCraft - Solução Completa

Conjunto de bibliotecas .NET para abstração e manipulação de bancos de dados SQL, com suporte a múltiplos bancos e integração com [SqlKata](https://github.com/sqlkata/querybuilder).

## Projetos incluídos

- **MVFC.SQLCraft**: Biblioteca base de abstração e utilitários SQL.
- **MVFC.SQLCraft.Mysql**: Driver para MySQL/MariaDB.
- **MVFC.SQLCraft.MsSQL**: Driver para Microsoft SQL Server.
- **MVFC.SQLCraft.PostgreSql**: Driver para PostgreSQL.
- **MVFC.SQLCraft.SQLite**: Driver para SQLite.
- **MVFC.SQLCraft.Firebird**: Driver para Firebird.
- **MVFC.SQLCraft.Oracle**: Driver para Oracle.

## Instalação

Cada driver é distribuído como pacote NuGet independente.  
Exemplos:

```sh
dotnet add package MVFC.SQLCraft
dotnet add package MVFC.SQLCraft.Mysql
dotnet add package MVFC.SQLCraft.Firebird
dotnet add package MVFC.SQLCraft.MsSQL
dotnet add package MVFC.SQLCraft.PostgreSql
dotnet add package MVFC.SQLCraft.SQLite
dotnet add package MVFC.SQLCraft.Oracle
```

Repita para o driver desejado.

## Como usar

1. Instale o pacote base e o driver do banco desejado.
2. Utilize as abstrações do MVFC.SQLCraft para criar conexões e executar queries.

## Sobre

- Compatível com .NET 9.0+
- Licença MIT
- Cada projeto possui seu próprio README e documentação específica.

## Licença

MIT