namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class TransactionTypeRepository : IQueryRepository<SalesLedgerTransactionType>
    {
        private readonly ServiceDbContext serviceDbContext;

        public TransactionTypeRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public SalesLedgerTransactionType FindBy(Expression<Func<SalesLedgerTransactionType, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SalesLedgerTransactionType> FilterBy(Expression<Func<SalesLedgerTransactionType, bool>> expression)
        {
            return this.serviceDbContext.TransactionTypes.Where(expression);
        }

        public IQueryable<SalesLedgerTransactionType> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}