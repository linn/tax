namespace Linn.Tax.Proxy
{
    using System.Data;

    using Linn.Tax.Domain;

    using Oracle.ManagedDataAccess.Client;

    public class DatabaseService : IDatabaseService
    {
        public DataSet ExecuteQuery(string sql)
        {
            using (var connection = this.GetConnection())
            {
                var dataAdapter = new OracleDataAdapter(
                    new OracleCommand(sql, connection) { CommandType = CommandType.Text });
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(ConnectionStrings.ManagedConnectionString());
        }
    }
}