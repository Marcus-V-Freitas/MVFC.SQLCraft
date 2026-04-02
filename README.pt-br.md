# MVFC.SQLCraft

> 🇺🇸 [Read in English](README.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft)

Suite completa de bibliotecas .NET para abstração e manipulação fluente de bancos de dados SQL, com suporte a múltiplos bancos e integração com [SqlKata](https://github.com/sqlkata/querybuilder).

## Motivação

Trabalhar com múltiplos bancos SQL no .NET normalmente implica em:

- Boilerplate repetitivo de gerenciamento e descarte de conexões.
- Configuração manual de drivers ADO.NET distintos por banco de dados.
- Duplicação da lógica de transações em cada repositório.
- Ausência de observabilidade sem um hook de logging consistente.
- Mistura inconsistente de SQL puro com query builders.

**MVFC.SQLCraft** resolve isso fornecendo uma classe base abstrata única que cada pacote específico por banco estende. Você escolhe o driver do seu banco, herda dele e obtém uma API consistente e testada para consultas, execuções e gerenciamento de transações — com logging estruturado opcional integrado.

## Arquitetura

Todos os pacotes seguem o mesmo padrão:

- `SQLCraftDriver` (classe base) — gerencia conexões, ciclo de vida do logging e escopo de transação.
- `XxxCraftDriver` (classe de driver por banco) — fornece a factory de `DbConnection` e o `Compiler` SqlKata para o dialeto SQL.
- `IDatabaseLogger` (interface opcional) — intercepta SQL antes/depois da execução e em caso de erros.
- `IQueryFactory` / `DefaultQueryFactory` — wrapper sobre o `QueryFactory` do SqlKata.

Aprendendo a usar um driver, todos os outros funcionam de forma idêntica.

## Pacotes

| Pacote | Banco de Dados | Downloads |
|---|---|---|
| [MVFC.SQLCraft](src/MVFC.SQLCraft/README.md) | Abstrações base | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft) |
| [MVFC.SQLCraft.Mysql](src/MVFC.SQLCraft.Mysql/README.md) | MySQL / MariaDB | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Mysql) |
| [MVFC.SQLCraft.MsSQL](src/MVFC.SQLCraft.MsSQL/README.md) | Microsoft SQL Server | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.MsSQL) |
| [MVFC.SQLCraft.PostgreSql](src/MVFC.SQLCraft.PostgreSql/README.md) | PostgreSQL | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.PostgreSql) |
| [MVFC.SQLCraft.SQLite](src/MVFC.SQLCraft.SQLite/README.md) | SQLite | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.SQLite) |
| [MVFC.SQLCraft.Firebird](src/MVFC.SQLCraft.Firebird/README.md) | Firebird | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Firebird) |
| [MVFC.SQLCraft.Oracle](src/MVFC.SQLCraft.Oracle/README.md) | Oracle | ![Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft.Oracle) |

***

## Instalação

Instale o pacote base e o driver do banco desejado:

```sh
# Abstrações base (obrigatório)
dotnet add package MVFC.SQLCraft

# Escolha seu driver
dotnet add package MVFC.SQLCraft.Mysql
dotnet add package MVFC.SQLCraft.MsSQL
dotnet add package MVFC.SQLCraft.PostgreSql
dotnet add package MVFC.SQLCraft.SQLite
dotnet add package MVFC.SQLCraft.Firebird
dotnet add package MVFC.SQLCraft.Oracle
```

## Início Rápido

### 1. Crie seu driver
Herde do driver específico do banco e passe sua connection string:

```csharp
using MVFC.SQLCraft.Mysql;

public class MinhaDatabase : MysqlCraftDriver
{
    public MinhaDatabase(string connectionString)
        : base(connectionString, logger: null) { }
}
```

### 2. Registre no DI

```csharp
builder.Services.AddSingleton<MinhaDatabase>(_ =>
    new MinhaDatabase(builder.Configuration.GetConnectionString("Default")!));
```

### 3. Consulte de forma fluente

```csharp
public class UsuarioRepository
{
    private readonly MinhaDatabase _db;

    public UsuarioRepository(MinhaDatabase db) => _db = db;

    public async Task<IEnumerable<Usuario>> ObterUsuariosAtivosAsync(CancellationToken ct = default)
    {
        var query = new Query("usuarios")
            .Where("ativo", true)
            .OrderByDesc("criado_em")
            .Limit(100);

        return await _db.QueryAsync<Usuario>(query, ct: ct);
    }

    public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var query = new Query("usuarios").Where("id", id);
        return await _db.QueryFirstOrDefaultAsync<Usuario>(query, ct: ct);
    }
}
```

## API Disponível
Todos os métodos estão definidos em SQLCraftDriver e disponíveis em cada driver.

### Métodos de Consulta

| Método | Descrição |
|---|---|
| Query<T>(query, tx?) | Retorna todas as linhas correspondentes |
| QueryAsync<T>(query, tx?, ct) | Versão assíncrona de Query<T> |
| QueryFirstOrDefault<T>(query, tx?) | Retorna a primeira linha ou null |
| QueryFirstOrDefaultAsync<T>(query, tx?, ct) | Versão assíncrona |

### Métodos de Execução

| Método | Descrição |
|---|---|
| Execute(query, tx?) | Executa um Query SqlKata (INSERT/UPDATE/DELETE) |
| Execute(sql, tx?) | Executa uma string SQL pura |
| ExecuteAsync(query, tx?, ct) | Versão assíncrona para Query |
| ExecuteAsync(sql, tx?, ct) | Versão assíncrona para SQL puro |

### Métodos de Transação

| Método | Descrição |
|---|---|
| ExecuteInTransaction(action, isolation?) | Envolve uma ação síncrona em uma transação gerenciada |
| ExecuteInTransactionAsync(action, isolation?, ct) | Versão assíncrona — commit no sucesso, rollback em exceção |

## Transações
Os wrappers de transação gerenciam o ciclo de vida completo (abertura, commit, rollback e dispose) de forma automática:

```csharp
await _db.ExecuteInTransactionAsync(async (driver, tx, ct) =>
{
    var inserirPedido = new Query("pedidos").AsInsert(new { ClienteId = 42, Total = 199.90m });
    await driver.ExecuteAsync(inserirPedido, tx, ct);

    var atualizarEstoque = new Query("produtos").Where("id", 7).AsDecrement("estoque", 1);
    await driver.ExecuteAsync(atualizarEstoque, tx, ct);
}, isolation: IsolationLevel.ReadCommitted, ct: cancellationToken);
```

Se qualquer operação lançar uma exceção, a transação é automaticamente revertida.

## Logging Estruturado
Implemente IDatabaseLogger para interceptar todas as execuções SQL:

```csharp
public class MeuDatabaseLogger : IDatabaseLogger
{
    private readonly ILogger<MeuDatabaseLogger> _logger;

    public MeuDatabaseLogger(ILogger<MeuDatabaseLogger> logger) => _logger = logger;

    public Task OnBeforeExecuteAsync(string sql, object? bindings, CancellationToken ct = default)
    {
        _logger.LogDebug("Executando SQL: {Sql} | Parâmetros: {Bindings}", sql, bindings);
        return Task.CompletedTask;
    }

    public Task OnAfterExecuteAsync(string sql, object? bindings, TimeSpan elapsed, CancellationToken ct = default)
    {
        _logger.LogInformation("SQL executado em {Elapsed}ms: {Sql}", elapsed.TotalMilliseconds, sql);
        return Task.CompletedTask;
    }

    public Task OnErrorAsync(string sql, object? bindings, Exception ex, CancellationToken ct = default)
    {
        _logger.LogError(ex, "Falha no SQL: {Sql} | Parâmetros: {Bindings}", sql, bindings);
        return Task.CompletedTask;
    }
}
```

Em seguida, registre no driver:

```csharp
public class MinhaDatabase : MysqlCraftDriver
{
    public MinhaDatabase(string connectionString, MeuDatabaseLogger logger)
        : base(connectionString, logger) { }
}
```

## Estrutura do Projeto

```text
src/
  MVFC.SQLCraft/               # Abstrações base (SQLCraftDriver, IDatabaseLogger, IQueryFactory)
  MVFC.SQLCraft.Firebird/      # Driver Firebird
  MVFC.SQLCraft.MsSQL/         # Driver SQL Server
  MVFC.SQLCraft.Mysql/         # Driver MySQL / MariaDB
  MVFC.SQLCraft.Oracle/        # Driver Oracle
  MVFC.SQLCraft.PostgreSql/    # Driver PostgreSQL
  MVFC.SQLCraft.SQLite/        # Driver SQLite
tests/
  # Testes unitários e de integração
tools/
  # Ferramentas de build (Cake)
```

## Requisitos
.NET 9.0+

SqlKata >= 2.x

O provider ADO.NET correspondente a cada banco (obtido automaticamente via NuGet)

## Contribuindo

Veja [CONTRIBUTING.md](CONTRIBUTING.md).

## Licença

[Apache-2.0](LICENSE)
