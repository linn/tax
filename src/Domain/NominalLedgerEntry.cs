namespace Linn.Tax.Domain
{
    using System;

    public class NominalLedgerEntry
    {
        public int Tref { get; set; }

        public int PeriodNumber { get; set; }

        public decimal Amount { get; set; }

        public string Comments { get; set; }

        public string TransactionType { get; set; }

        public int NominalAccountId { get; set; }

        public int JournalNumber { get; set; }

        public DateTime DatePosted { get; set; }

        public string CreditOrDebit { get; set; }

        public string Narrative { get; set; }

        public string Description { get; set; }
    }
}
