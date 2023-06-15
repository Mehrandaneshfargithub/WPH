using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwReturnPurchaseInvoice
    {
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public int? ReasonId { get; set; }
        public string Username { get; set; }
        public decimal ReturnDiscount { get; set; }
        public string ReasonName { get; set; }
        public string MainInvoiceNum { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
