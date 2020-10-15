namespace Linn.Tax.Domain
{
    public class Purchase
    {
        public string TransactionType { get; set; }

        public string LedgerPeriod { get; set; }

        public decimal NetTotal { get; set; }

        public decimal VatTotal { get; set; }
    }
}