namespace Linn.Tax.Domain
{
    using System.Data;

    public interface IDatabaseService
    {
        DataSet ExecuteQuery(string sql);
    }
}
