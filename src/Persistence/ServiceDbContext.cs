namespace Linn.Tax.Persistence
{
    using Linn.Common.Configuration;
    using Linn.Tax.Domain;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[]
                                  {
                                      new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                                  });

        public DbQuery<LedgerMaster> LedgerMaster { get; set; }

        public DbQuery<SalesLedgerEntry> LedgerEntries { get; set; }

        public DbQuery<SalesLedgerTransactionType> TransactionTypes { get; set; }

        public DbQuery<Purchase> PurchaseLedger { get; set; }

        public DbQuery<PurchaseLedgerTransactionType> PurchaseLedgerTransactionTypes { get; set; }

        public DbQuery<Supplier> Suppliers { get; set; }

        public DbQuery<NominalLedgerEntry> NominalLedger { get; set; }

        public DbSet<VatReturnReceipt> VatReturnReceipts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.QueryMasterLedger(builder);
            this.QueryLedgerEntries(builder);
            this.QueryTransactionTypes(builder);
            this.QueryPurchaseLedger(builder);
            this.QueryPurchaseLedgerTransactionTypes(builder);
            this.QuerySuppliers(builder);
            this.QueryNominalLedger(builder);
            this.BuildVatReturnReceipts(builder);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = ConfigurationManager.Configuration["DATABASE_HOST"];
            var userId = ConfigurationManager.Configuration["DATABASE_USER_ID"];
            var password = ConfigurationManager.Configuration["DATABASE_PASSWORD"];
            var serviceId = ConfigurationManager.Configuration["DATABASE_NAME"];

            var dataSource =
                $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={serviceId})(SERVER=dedicated)))";

            optionsBuilder.UseOracle($"Data Source={dataSource};User Id={userId};Password={password};");
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        private void QueryMasterLedger(ModelBuilder builder)
        {
            builder.Query<LedgerMaster>().ToView("SL_LEDGER_MASTER");
            builder.Query<LedgerMaster>().Property(l => l.CurrentPeriod).HasColumnName("CURRENT_LEDGER_PERIOD");
        }

        private void QueryLedgerEntries(ModelBuilder builder)
        {
            builder.Query<SalesLedgerEntry>().ToView("SL_LEDGER_ENTRIES");
            builder.Query<SalesLedgerEntry>().Property(e => e.LedgerPeriod).HasColumnName("LEDGER_PERIOD");
            builder.Query<SalesLedgerEntry>().Property(e => e.LedgerId).HasColumnName("LEDGER_ID");
            builder.Query<SalesLedgerEntry>().Property(e => e.BaseNetAmount).HasColumnName("BASE_NET_AMOUNT");
            builder.Query<SalesLedgerEntry>().Property(e => e.BaseVatAmount).HasColumnName("BASE_VAT_AMOUNT");
            builder.Query<SalesLedgerEntry>().Property(e => e.TransactionType).HasColumnName("TRANS_TYPE");
        }

        private void QueryTransactionTypes(ModelBuilder builder)
        {
            builder.Query<SalesLedgerTransactionType>().ToView("SL_TRANS_TYPES");
            builder.Query<SalesLedgerTransactionType>().Property(t => t.TransactionName).HasColumnName("TRANSACTION_NAME");
        }

        private void QueryPurchaseLedger(ModelBuilder builder)
        {
            builder.Query<Purchase>().ToView("PURCHASE_LEDGER");
            builder.Query<Purchase>().Property(p => p.LedgerPeriod).HasColumnName("LEDGER_PERIOD");
            builder.Query<Purchase>().Property(p => p.NetTotal).HasColumnName("BASE_NET_TOTAL");
            builder.Query<Purchase>().Property(p => p.VatTotal).HasColumnName("BASE_VAT_TOTAL");
            builder.Query<Purchase>().Property(p => p.TransactionType).HasColumnName("PL_TRANS_TYPE");
            builder.Query<Purchase>().Property(p => p.SupplierId).HasColumnName("SUPPLIER_ID");
        }

        private void QueryPurchaseLedgerTransactionTypes(ModelBuilder builder)
        {
            builder.Query<PurchaseLedgerTransactionType>().ToView("PL_TRANS_TYPES");
            builder.Query<PurchaseLedgerTransactionType>().Property(t => t.TransactionType)
                .HasColumnName("PL_TRANS_TYPE");
            builder.Query<PurchaseLedgerTransactionType>().Property(t => t.TransactionCategory)
                .HasColumnName("TRANS_CATEGORY");
            builder.Query<PurchaseLedgerTransactionType>().Property(t => t.DebitOrCredit)
                .HasColumnName("DEBIT_OR_CREDIT");
        }

        private void QuerySuppliers(ModelBuilder builder)
        {
            builder.Query<Supplier>().ToView("SUPPLIERS");
            builder.Query<Supplier>().Property(s => s.SupplierId).HasColumnName("SUPPLIER_ID");
            builder.Query<Supplier>().Property(s => s.LiveOnOracle).HasColumnName("LIVE_ON_ORACLE");
        }

        private void QueryNominalLedger(ModelBuilder builder)
        {
           builder.Query<NominalLedgerEntry>().ToView("NOMINAL_LEDGER");
           builder.Query<NominalLedgerEntry>().Property(e => e.Amount).HasColumnName("AMOUNT");
           builder.Query<NominalLedgerEntry>().Property(e => e.TransactionType).HasColumnName("TRANS_TYPE");
           builder.Query<NominalLedgerEntry>().Property(e => e.Comments).HasColumnName("COMMENTS");
           builder.Query<NominalLedgerEntry>().Property(e => e.NominalAccountId).HasColumnName("NOMACC_ID");
           builder.Query<NominalLedgerEntry>().Property(e => e.PeriodNumber).HasColumnName("PERIOD_NUMBER");
           builder.Query<NominalLedgerEntry>().Property(e => e.JournalNumber).HasColumnName("JOURNAL_NUMBER");
           builder.Query<NominalLedgerEntry>().Property(e => e.CreditOrDebit).HasColumnName("CREDIT_OR_DEBIT");
        }

        private void BuildVatReturnReceipts(ModelBuilder builder)
        {
           builder.Entity<VatReturnReceipt>().ToTable("VAT_RETURN_RECEIPTS");
           builder.Entity<VatReturnReceipt>().HasKey(r => r.FormBundleNumber);
           builder.Entity<VatReturnReceipt>().Property(r => r.ProcessingDate).HasColumnName("PROCESSING_DATE");
           builder.Entity<VatReturnReceipt>().Property(r => r.FormBundleNumber).HasColumnName("FORM_BUNDLE_NUMBER");
           builder.Entity<VatReturnReceipt>().Property(r => r.ChargeRefNumber).HasColumnName("CHARGE_REF_NUMBER");
           builder.Entity<VatReturnReceipt>().Property(r => r.PaymentIndicator).HasColumnName("PAYMENT_INDICATOR");
        }
    }
}
