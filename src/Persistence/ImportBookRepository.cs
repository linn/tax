namespace Linn.Tax.Persistence
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Tax.Domain;

    public class ImportBookRepository : IQueryRepository<ImportBook>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ImportBookRepository(ServiceDbContext serviceDbContext)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public ImportBook FindBy(Expression<Func<ImportBook, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ImportBook> FilterBy(Expression<Func<ImportBook, bool>> expression)
        {
            return this.serviceDbContext.ImportBooks.Where(expression);
        }

        public IQueryable<ImportBook> FindAll()
        {
            throw new NotImplementedException();
        }
    }
}
