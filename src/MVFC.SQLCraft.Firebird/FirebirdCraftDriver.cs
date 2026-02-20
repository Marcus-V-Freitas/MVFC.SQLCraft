namespace MVFC.SQLCraft.Firebird;

public sealed class FirebirdCraftDriver(string connectionString, IDatabaseLogger? logger = null) : SQLCraftDriver(connectionString, logger) {
    protected override Compiler Compiler => new FirebirdCompiler();
    protected override DbConnection ConnectionFactory() => new FbConnection(_connectionString);
}