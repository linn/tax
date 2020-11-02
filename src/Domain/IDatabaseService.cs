namespace Linn.Tax.Domain
{
    using System.Data;

    // using Oracle.ManagedDataAccess.Client;

    public interface IDatabaseService
    {
        // OracleConnection GetConnection();

        DataSet ExecuteQuery(string sql);
    }
}