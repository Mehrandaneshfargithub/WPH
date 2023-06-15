using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwShantyonPurchaseInvoice
    {
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? Num { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Name { get; set; }
        public string JoineryName { get; set; }
    }
}
