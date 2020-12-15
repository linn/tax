namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class LedgerPeriodRepository : IQueryRepository<LedgerPeriod>
    {
        private readonly ServiceDbContext serviceDbContext;

        public LedgerPeriodRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public LedgerPeriod FindBy(Expression<Func<LedgerPeriod, bool>> expression)
        {
            return this.serviceDbContext.LedgerPeriods
                .Where(expression).ToList().FirstOrDefault();
        }

        public IQueryable<LedgerPeriod> FilterBy(Expression<Func<LedgerPeriod, bool>> expression)
        {
            return this.serviceDbContext.LedgerPeriods.Where(expression);
        }

        public IQueryable<LedgerPeriod> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}