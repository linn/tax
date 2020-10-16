namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class PurchaseLedgerRepository : IQueryRepository<Purchase>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseLedgerRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public Purchase FindBy(Expression<Func<Purchase, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Purchase> FilterBy(Expression<Func<Purchase, bool>> expression)
        {
            return this.serviceDbContext.PurchaseLedger.Where(expression);
        }

        public IQueryable<Purchase> FindAll()
        {
            return this.serviceDbContext.PurchaseLedger;
        }
    }
}
