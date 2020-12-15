namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class VatReturnReceiptRepository : IRepository<VatReturnReceipt, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public VatReturnReceiptRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public void Add(VatReturnReceipt entity)
        {
            this.serviceDbContext.VatReturnReceipts.Add(entity);
            this.serviceDbContext.SaveChanges();
        }

        public IQueryable<VatReturnReceipt> FilterBy(Expression<Func<VatReturnReceipt, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VatReturnReceipt> FindAll()
        {
            throw new NotImplementedException();
        }

        public VatReturnReceipt FindBy(Expression<Func<VatReturnReceipt, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public VatReturnReceipt FindById(int key)
        {
            throw new NotImplementedException();
        }

        public void Remove(VatReturnReceipt entity)
        {
            throw new NotImplementedException();
        }
    }
}
