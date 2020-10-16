namespace Linn.Tax.IoC
{
    using Autofac;
    using Linn.Common.Persistence;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Tax.Domain;
    using Linn.Tax.Persistence;

    using Microsoft.EntityFrameworkCore;

    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceDbContext>().AsSelf()
                .As<DbContext>().InstancePerRequest();
            builder.RegisterType<TransactionManager>().As<ITransactionManager>();

            builder.RegisterType<LedgerEntryRepository>().As<IQueryRepository<LedgerEntry>>();
            builder.RegisterType<LedgerMasterRepository>().As<IQueryRepository<LedgerMaster>>();
            builder.RegisterType<PurchaseLedgerRepository>().As<IQueryRepository<Purchase>>();
            builder.RegisterType<SupplierRepository>().As<IQueryRepository<Supplier>>();
            builder.RegisterType<PurchaseLedgerTransactionTypeRepository>()
                .As<IQueryRepository<PurchaseLedgerTransactionType>>();
        }
    }
}