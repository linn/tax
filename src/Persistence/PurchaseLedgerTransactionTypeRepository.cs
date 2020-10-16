namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class PurchaseLedgerTransactionTypeRepository : IQueryRepository<PurchaseLedgerTransactionType>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseLedgerTransactionTypeRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public PurchaseLedgerTransactionType FindBy(Expression<Func<PurchaseLedgerTransactionType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PurchaseLedgerTransactionType> FilterBy(Expression<Func<PurchaseLedgerTransactionType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PurchaseLedgerTransactionType> FindAll()
        {
            return this.serviceDbContext.PurchaseLedgerTransactionTypes;
        }
    }
}