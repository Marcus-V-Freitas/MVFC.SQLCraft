namespace MVFC.SQLCraft.MsSQL;

public sealed class MsSQLCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger) {
    protected override Compiler Compiler => new SqlServerCompiler();

    protected override DbConnection ConnectionFactory() => new SqlConnection(_connectionString);
}
