namespace Linn.Tax.Domain
{
    public class Purchase
    {
        public string TransactionType { get; set; }

        public int LedgerPeriod { get; set; }

        public decimal NetTotal { get; set; }

        public decimal VatTotal { get; set; }

        public int SupplierId { get; set; }
    }
}