namespace Linn.Tax.Domain.Tests.VatReturnCalculationServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<SalesLedgerEntry> LedgerEntryRepository { get; private set; }

        protected IQueryRepository<Purchase> PurchaseLedger { get; private set; }

        protected IQueryRepository<PurchaseLedgerTransactionType> PurchaseLedgerTransactionTypeRepository
        {
            get; private set;
        }

        protected IQueryRepository<Supplier> SupplierRepository { get; private set; }

        protected IQueryRepository<LedgerMaster> LedgerMasterRepository { get; private set; }

        protected IQueryRepository<LedgerPeriod> LedgerPeriodRepository { get; set; }

        protected IVatReturnCalculationService Sut { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.LedgerEntryRepository = Substitute.For<IQueryRepository<SalesLedgerEntry>>();
            this.PurchaseLedger = Substitute.For<IQueryRepository<Purchase>>();
            this.PurchaseLedgerTransactionTypeRepository =
                Substitute.For<IQueryRepository<PurchaseLedgerTransactionType>>();
            this.SupplierRepository = Substitute.For<IQueryRepository<Supplier>>();
            this.LedgerMasterRepository = Substitute.For<IQueryRepository<LedgerMaster>>();
            this.DatabaseService = Substitute.For<IDatabaseService>();
            this.LedgerPeriodRepository = Substitute.For<IQueryRepository<LedgerPeriod>>();

            this.LedgerMasterRepository.FindAll()
                .Returns(new List<LedgerMaster> { new LedgerMaster { CurrentPeriod = 1000 } }.AsQueryable());

            this.Sut = new VatReturnCalculationService(
                this.LedgerEntryRepository,
                this.PurchaseLedger,
                this.PurchaseLedgerTransactionTypeRepository,
                this.SupplierRepository,
                this.LedgerMasterRepository,
                this.LedgerPeriodRepository,
                this.DatabaseService);
        }
    }
}
