using System;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoicePay
    {
        public Guid Guid { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? PayId { get; set; }
        public bool? FullPay { get; set; }

        public virtual PurchaseInvoice Invoice { get; set; }
        public virtual Pay Pay { get; set; }
    }
}
