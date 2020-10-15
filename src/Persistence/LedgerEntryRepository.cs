namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class LedgerEntryRepository : IQueryRepository<LedgerEntry>
    {
        private readonly ServiceDbContext serviceDbContext;

        public LedgerEntryRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public LedgerEntry FindBy(Expression<Func<LedgerEntry, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LedgerEntry> FilterBy(Expression<Func<LedgerEntry, bool>> expression)
        {
            return this.serviceDbContext.LedgerEntries.Where(expression);
        }

        public IQueryable<LedgerEntry> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}