namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class LedgerEntryRepository : IQueryRepository<SalesLedgerEntry>
    {
        private readonly ServiceDbContext serviceDbContext;

        public LedgerEntryRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public SalesLedgerEntry FindBy(Expression<Func<SalesLedgerEntry, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SalesLedgerEntry> FilterBy(Expression<Func<SalesLedgerEntry, bool>> expression)
        {
            return this.serviceDbContext.LedgerEntries.Where(expression);
        }

        public IQueryable<SalesLedgerEntry> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
