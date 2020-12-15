namespace Linn.Tax.Domain
{
    public class NominalLedgerEntry
    {
        public int PeriodNumber { get; set; }

        public decimal Amount { get; set; }

        public string Comments { get; set; }

        public string TransactionType { get; set; }

        public int NominalAccountId { get; set; }

        public int JournalNumber { get; set; }

        public string CreditOrDebit { get; set; }
    }
}
