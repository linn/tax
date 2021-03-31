namespace Linn.Tax.Resources
{
    public class NominalLedgerEntryResource
    {
        public int Tref { get; set; }

        public string DatePosted { get; set; }

        public decimal Amount { get; set; }

        public string CreditOrDebit { get; set; }

        public string Narrative { get; set; }

        public string Description { get; set; }

        public string Comments { get; set; }
    }
}
