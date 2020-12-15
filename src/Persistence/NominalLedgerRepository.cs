namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class NominalLedgerRepository : IQueryRepository<NominalLedgerEntry>
    {
        private readonly ServiceDbContext serviceDbContext;

        public NominalLedgerRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public NominalLedgerEntry FindBy(Expression<Func<NominalLedgerEntry, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<NominalLedgerEntry> FilterBy(Expression<Func<NominalLedgerEntry, bool>> expression)
        {
            return this.serviceDbContext.NominalLedger.Where(expression);
        }

        public IQueryable<NominalLedgerEntry> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
