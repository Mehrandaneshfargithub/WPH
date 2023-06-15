using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPurchaseInvoicePay
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PayId { get; set; }
        public bool? FullPay { get; set; }
    }
}
