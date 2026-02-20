using MVFC.SQLCraft.Servicos.Logs;

namespace MVFC.SQLCraft.Mysql;

public sealed class MysqlCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger)
{
    protected override Compiler Compiler => new MySqlCompiler();

    protected override DbConnection ConnectionFactory() => new MySqlConnection(_connectionString);
}