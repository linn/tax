namespace Linn.Tax.Domain
{
    using System;

    public class VatReturnReceipt
    {
        public int Id { get; set; }

        public DateTime ProcessingDate { get; set; }
        
        public string PaymentIndicator { get; set; }
        
        public string FormBundleNumber { get; set; }
        
        public string ChargeRefNumber { get; set; }
    }
}
