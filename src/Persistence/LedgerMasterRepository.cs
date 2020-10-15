namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class LedgerMasterRepository : IQueryRepository<LedgerMaster>
    {
        private readonly ServiceDbContext serviceDbContext;

        public LedgerMasterRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public LedgerMaster FindBy(Expression<Func<LedgerMaster, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LedgerMaster> FilterBy(Expression<Func<LedgerMaster, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<LedgerMaster> FindAll()
        {
            return this.serviceDbContext.LedgerMaster;
        }
    }
}