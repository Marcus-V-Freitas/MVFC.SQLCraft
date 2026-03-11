namespace MVFC.SQLCraft.Tests.CustomDriver;

public sealed class CustomTestCraftDriverTests(CustomFixture fixture) : IClassFixture<CustomFixture>
{
    private readonly CustomFixture _fixture = fixture;
    private readonly CustomTestCraftDriver _driver = new(fixture.ConnectionString);

    private CustomTestCraftDriver CriarDriver(CustomTestLogger? logger = null)
        => new(_fixture.ConnectionString, logger);

    [Fact]
    public void ExecutarEmTransacao_Confirma_QuandoSemExcecao()
    {
        _driver.Execute("CREATE TABLE IF NOT EXISTS tx_test (id SERIAL PRIMARY KEY, name VARCHAR(100));");
        _driver.ExecuteInTransaction((drv, tx) => drv.Execute(new Query("tx_test").AsInsert(new { name = "TxCommit" }), tx));

        var person = _driver.QueryFirstOrDefault<Person>(new Query("tx_test").Where("name", "TxCommit"));
        person.Should().NotBeNull();
        person!.Name.Should().Be("TxCommit");
    }

    [Fact]
    public void ExecutarEmTransacao_Desfaz_QuandoExcecao()
    {
        _driver.Execute("CREATE TABLE IF NOT EXISTS tx_test2 (id SERIAL PRIMARY KEY, name VARCHAR(100));");

        var action = () => _driver.ExecuteInTransaction((drv, tx) =>
        {
            drv.Execute(new Query("tx_test2").AsInsert(new { name = "TxRollback" }), tx);
            throw new InvalidOperationException();
        });

        action.Should().Throw<InvalidOperationException>();

        var person = _driver.QueryFirstOrDefault<Person>(new Query("tx_test2").Where("name", "TxRollback"));
        person.Should().BeNull();
    }

    [Fact]
    public async Task ExecutarEmTransacaoAsync_Confirma_QuandoSemExcecao()
    {
        await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS tx_test_async (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);

        await _driver.ExecuteInTransactionAsync(async (drv, tx, ct) =>
            await drv.ExecuteAsync(new Query("tx_test_async").AsInsert(new { name = "TxCommitAsync" }), tx, ct)
                     .ConfigureAwait(true), ct: TestContext.Current.CancellationToken);

        var person = await _driver.QueryFirstOrDefaultAsync<Person>(new Query("tx_test_async").Where("name", "TxCommitAsync"), ct: TestContext.Current.CancellationToken);

        person.Should().NotBeNull();
        person!.Name.Should().Be("TxCommitAsync");
    }

    [Fact]
    public async Task ExecutarEmTransacaoAsync_Desfaz_QuandoExcecao()
    {
        await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS tx_test_async2 (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);

        var action = async () => await _driver.ExecuteInTransactionAsync(async (drv, tx, ct) =>
        {
            await drv.ExecuteAsync(new Query("tx_test_async2").AsInsert(new { name = "TxRollbackAsync" }), tx, ct).ConfigureAwait(true);
            throw new InvalidOperationException();
        }, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);

        await action.Should().ThrowAsync<InvalidOperationException>();

        var person = await _driver.QueryFirstOrDefaultAsync<Person>(new Query("tx_test_async2").Where("name", "TxRollbackAsync"), ct: TestContext.Current.CancellationToken);
        person.Should().BeNull();
    }

    [Fact]
    public void ExecutarSqlDireto_Funciona()
    {
        var affected = _driver.Execute("CREATE TABLE IF NOT EXISTS direct_sql_test (id SERIAL PRIMARY KEY, name VARCHAR(100));");
        affected.Should().Be(-1);
    }

    [Fact]
    public async Task ExecutarSqlDiretoAsync_Funciona()
    {
        var affected = await _driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS direct_sql_test_async (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);
        affected.Should().Be(-1);
    }

    [Fact]
    public void Consultar_LancaELogaErro_QuandoTabelaNaoExiste()
    {
        var query = new Query("table_inexistente").Select("id", "name");
        var action = () => _driver.Query<Person>(query);
        action.Should().Throw<Exception>();
    }

    [Fact]
    public async Task ConsultarAsync_LancaELogaErro_QuandoTabelaNaoExiste()
    {
        var query = new Query("table_inexistente_async").Select("id", "name");
        var action = async () => await _driver.QueryAsync<Person>(query, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public void Executar_LancaELogaErro_QuandoSqlInvalido()
    {
        var action = () => _driver.Execute("INVALID SQL COMMAND");
        action.Should().Throw<Exception>();
    }

    [Fact]
    public async Task ExecutarAsync_LancaELogaErro_QuandoSqlInvalido()
    {
        var action = async () => await _driver.ExecuteAsync("INVALID SQL COMMAND", ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public void DisposeConexao_Chamado_EmConsulta()
    {
        var driver = CriarDriver();
        var action = () => driver.Query<Person>(new Query("table_inexistente"));
        action.Should().Throw<Exception>();
        driver.DisposeConnCalled.Should().BeTrue();
    }

    [Fact]
    public async Task DisposeConexaoAsync_Chamado_EmConsultaAsync()
    {
        var driver = CriarDriver();
        var action = async () => await driver.QueryAsync<Person>(new Query("table_inexistente_async"), ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action.Should().ThrowAsync<Exception>();
        driver.DisposeConnAsyncCalled.Should().BeTrue();
    }

    [Fact]
    public void LogAntesEDepois_Chamados_EmExecucao()
    {
        var driver = CriarDriver();
        driver.Execute("CREATE TABLE IF NOT EXISTS log_test_full (id SERIAL PRIMARY KEY, name VARCHAR(100));");
        driver.Execute(new Query("log_test_full").AsInsert(new { name = "TestFull" }));
        driver.LogBeforeCalled.Should().BeTrue();
        driver.LogAfterCalled.Should().BeTrue();
    }

    [Fact]
    public void LogErro_Chamado_EmErroExecucao()
    {
        var driver = CriarDriver();
        var action = () => driver.Execute("INVALID SQL");
        action.Should().Throw<Exception>();
        driver.LogErrorCalled.Should().BeTrue();
    }

    [Fact]
    public void LogErro_Chamado_EmExcecaoConsulta()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);

        var query = new Query("table_inexistente").Select("id", "name");
        var action = () => driver.Query<Person>(query);
        action.Should().Throw<Exception>();
        logger.ErrorCalled.Should().BeTrue();
        logger.LastException.Should().NotBeNull();
        logger.LastSql.Should().NotBeNull();
    }

    [Fact]
    public async Task LogErroAsync_Chamado_EmExcecaoConsultaAsync()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);

        var query = new Query("table_inexistente_async").Select("id", "name");
        var action = async () => await driver.QueryAsync<Person>(query, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action.Should().ThrowAsync<Exception>();
        logger.ErrorCalled.Should().BeTrue();
        logger.LastException.Should().NotBeNull();
        logger.LastSql.Should().NotBeNull();
    }

    [Fact]
    public void LogAntesEDepois_Chamados_ComSucesso()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);

        driver.Execute("CREATE TABLE IF NOT EXISTS log_test (id SERIAL PRIMARY KEY, name VARCHAR(100));");
        var query = new Query("log_test").AsInsert(new { name = "Test" });
        driver.Execute(query);

        logger.BeforeCalled.Should().BeTrue();
        logger.AfterCalled.Should().BeTrue();
    }

    [Fact]
    public async Task LogAntesEDepoisAsync_Chamados_ComSucesso()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);

        await driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS log_test_async (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);
        var query = new Query("log_test_async").AsInsert(new { name = "TestAsync" });
        await driver.ExecuteAsync(query, ct: TestContext.Current.CancellationToken);

        logger.BeforeCalled.Should().BeTrue();
        logger.AfterCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ConsultarPrimeiroOuPadrao_Deve_LogarErro_EmExcecao()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);

        var query = new Query("tabela");
        await driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS tabela (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);
        driver.ThrowInternalError = true;

        var action1 = () => driver.Execute("SELECT 1");
        action1.Should().Throw<InvalidOperationException>();

        var action2 = () => driver.QueryFirstOrDefault<object>(query);
        action2.Should().Throw<InvalidOperationException>();

        var action3 = () => driver.Query<object>(query);
        action3.Should().Throw<InvalidOperationException>();

        var action4 = () => driver.Execute(query);
        action4.Should().Throw<InvalidOperationException>();

        var action5 = async () => await driver.QueryFirstOrDefaultAsync<object>(query, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action5.Should().ThrowAsync<InvalidOperationException>();

        var action6 = async () => await driver.QueryAsync<object>(query, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action6.Should().ThrowAsync<InvalidOperationException>();

        var action7 = async () => await driver.ExecuteAsync(query, ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action7.Should().ThrowAsync<InvalidOperationException>();

        var action8 = async () => await driver.ExecuteAsync("SELECT 1", ct: TestContext.Current.CancellationToken).ConfigureAwait(true);
        await action8.Should().ThrowAsync<InvalidOperationException>();

        logger.ErrorCalled.Should().BeTrue();
    }

    [Fact]
    public void Consultar_ComTransacao_CobreBranchTx()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);
        var query = new Query("tabela1");

        driver.Execute("CREATE TABLE IF NOT EXISTS tabela1 (id SERIAL PRIMARY KEY, name VARCHAR(100));");
        driver.ExecuteInTransaction((drv, tx) =>
        {
            drv.Query<object>(query, tx);
            drv.Execute(query, tx);
            drv.Execute("SELECT 1", tx);
            drv.QueryFirstOrDefault<object>(query, tx);
        });

        driver.LogAfterCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ConsultarAsync_ComTransacao_CobreBranchTx()
    {
        var logger = new CustomTestLogger();
        var driver = CriarDriver(logger);
        var query = new Query("table2");

        await driver.ExecuteAsync("CREATE TABLE IF NOT EXISTS table2 (id SERIAL PRIMARY KEY, name VARCHAR(100));", ct: TestContext.Current.CancellationToken);

        await driver.ExecuteInTransactionAsync(async (drv, tx, ct) =>
        {
            await drv.QueryAsync<object>(query, tx, ct).ConfigureAwait(true);
            await drv.ExecuteAsync(query, tx, ct).ConfigureAwait(true);
            await drv.QueryFirstOrDefaultAsync<object>(query, tx, ct).ConfigureAwait(true);
            await drv.ExecuteAsync("SELECT 1", tx, ct).ConfigureAwait(true);
        }, ct: TestContext.Current.CancellationToken);

        logger.AfterCalled.Should().BeTrue();
    }
}
