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

        public DbQuery<LedgerEntry> LedgerEntries { get; set; }

        public DbQuery<SalesLedgerTransactionType> TrasnsactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.QueryMasterLedger(builder);
            this.QueryLedgerEntries(builder);
            this.QueryTransactionTypes(builder);
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
            builder.Query<LedgerEntry>().ToView("SL_LEDGER_ENTRIES");
            builder.Query<LedgerEntry>().Property(e => e.LedgerPeriod).HasColumnName("LEDGER_PERIOD");
            builder.Query<LedgerEntry>().Property(e => e.LedgerId).HasColumnName("LEDGER_ID");
            builder.Query<LedgerEntry>().Property(e => e.BaseNetAmount).HasColumnName("BASE_NET_AMOUNT");
            builder.Query<LedgerEntry>().Property(e => e.BaseVatAmount).HasColumnName("BASE_VAT_AMOUNT");
            builder.Query<LedgerEntry>().Property(e => e.TransactionType).HasColumnName("TRANS_TYPE");
        }

        private void QueryTransactionTypes(ModelBuilder builder)
        {
            builder.Query<SalesLedgerTransactionType>().ToView("SL_TRANS_TYPES");
            builder.Query<SalesLedgerTransactionType>().Property(t => t.TransactionName).HasColumnName("TRANSACTION_NAME");
        }
    }
}