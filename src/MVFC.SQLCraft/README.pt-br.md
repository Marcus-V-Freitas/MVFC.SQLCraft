# MVFC.SQLCraft

> 🇺🇸 [Read in English](README.md)

[![CI](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml/badge.svg)](https://github.com/Marcus-V-Freitas/MVFC.SQLCraft/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft/branch/main/graph/badge.svg)](https://codecov.io/gh/Marcus-V-Freitas/MVFC.SQLCraft)
[![License](https://img.shields.io/badge/license-Apache--2.0-blue)](LICENSE)
![Platform](https://img.shields.io/badge/.NET-9%20%7C%2010-blue)
![NuGet Version](https://img.shields.io/nuget/v/MVFC.SQLCraft)
![NuGet Downloads](https://img.shields.io/nuget/dt/MVFC.SQLCraft)

Biblioteca base para abstração e manipulação de bancos de dados SQL no .NET 9.0+.  
Fornece interfaces, utilitários e integração com [SqlKata](https://github.com/sqlkata/querybuilder).

## Instalação

```sh
dotnet add package MVFC.SQLCraft
```

## Recursos

- Abstração de drivers SQL
- Integração com múltiplos bancos
- Factory para criação de conexões

## Principais Métodos

```csharp

// Retornar só um objeto
T? QueryFirstOrDefault<T>(Query query, IDbTransaction? tx = null);
Task<T?> QueryFirstOrDefaultAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default);

// Retornar múltiplos objetos
IEnumerable<T> Query<T>(Query query, IDbTransaction? tx = null);
Task<IEnumerable<T>> QueryAsync<T>(Query query, IDbTransaction? tx = null, CancellationToken ct = default);

// Executar uma query e retornar linhas afetadas
int Execute(Query query, IDbTransaction? tx = null);
Task<int> ExecuteAsync(Query query, IDbTransaction? tx = null, CancellationToken ct = default);

// Executar uma query bruta e retornar linhas afetadas
int Execute(string sql, IDbTransaction? tx = null);
Task<int> ExecuteAsync(string sql, IDbTransaction? tx = null, CancellationToken ct = default);

// Realizar uma transação
void ExecuteInTransaction(Action<SQLCraftDriver, IDbTransaction> action, IsolationLevel isolation = IsolationLevel.ReadCommitted);
Task ExecuteInTransactionAsync(Func<SQLCraftDriver, IDbTransaction, CancellationToken, Task> action, IsolationLevel isolation = IsolationLevel.ReadCommitted, CancellationToken ct = default);

```

### Exemplo de transação

```csharp

await driver.ExecuteInTransactionAsync(async (x, t, ct) => {
    
    await x.ExecuteAsync(new Query("Person")
                                .AsInsert(new 
                                { 
                                    Name = "Bob"
                                }), t, ct);

    var sel = new Query("Person")
                    .Select("Id", "Name")
                    .Where("Name", "Bob");

    var p = await x.QueryFirstOrDefaultAsync<Person>(sel, t, ct);
    Assert.NotNull(p);

    var upd = new Query("Person")
                    .Where("Id", p!.Id)
                    .AsUpdate(new 
                    { 
                        Name = "Robert" 
                     });
    
    var upaff = await x.ExecuteAsync(upd, t, ct);
    Assert.Equal(1, upaff);

    var sel2 = new Query("Person")
                    .Select("Id", "Name")
                    .Where("Id", p.Id);

    var p2 = await x.QueryFirstOrDefaultAsync<Person>(sel2, t, ct);
    
    Assert.NotNull(p2);
    Assert.Equal("Robert", p2!.Name);
});
```

## Licença

MIT
