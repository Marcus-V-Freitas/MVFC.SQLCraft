using MVFC.SQLCraft.Servicos.Logs;

namespace MVFC.SQLCraft.PostgreSql;

public sealed class PostgreSqlCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger)
{
    protected override Compiler Compiler => new PostgresCompiler();
    protected override DbConnection ConnectionFactory() => new NpgsqlConnection(_connectionString);
}