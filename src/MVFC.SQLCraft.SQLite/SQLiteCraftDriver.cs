using MVFC.SQLCraft.Servicos.Logs;

namespace MVFC.SQLCraft.SQLite;

public sealed class SQLiteCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger)
{
    protected override Compiler Compiler => new SqliteCompiler();
    protected override DbConnection ConnectionFactory() => new SQLiteConnection(_connectionString);
}
