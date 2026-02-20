namespace MVFC.SQLCraft.Servicos.Factory;

public sealed class DefaultQueryFactory(IDbConnection connection, Compiler compiler, int timeout = 30) :
    QueryFactory(connection, compiler, timeout), IQueryFactory;