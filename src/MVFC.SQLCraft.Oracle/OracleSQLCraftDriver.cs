namespace MVFC.SQLCraft.Oracle;

public sealed class OracleSQLCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger) {
    protected override Compiler Compiler => new OracleCompiler();

    protected override DbConnection ConnectionFactory() => new OracleConnection(_connectionString);
}
