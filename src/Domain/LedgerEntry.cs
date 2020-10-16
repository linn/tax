namespace Linn.Tax.Domain
{
    public class LedgerEntry
    {
        public decimal BaseNetAmount { get; set; }

        public decimal BaseVatAmount { get; set; }

        public string TransactionType { get; set; }

        public int LedgerId { get; set; } 

        public int LedgerPeriod { get; set; }
    }
}
