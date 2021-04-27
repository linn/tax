namespace Linn.Tax.Domain
{
    using System;

    public class ImportBook
    {
        public decimal LinnVat { get; set; }

        public string Pva { get; set; }

        public DateTime DateCreated { get; set; }

        public int PeriodNumber { get; set; }
    }
}
