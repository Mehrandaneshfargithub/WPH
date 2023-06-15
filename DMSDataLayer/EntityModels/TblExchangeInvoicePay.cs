using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblExchangeInvoicePay
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int PayId { get; set; }

        public virtual TblExchangeInvoice Invoice { get; set; }
        public virtual TblMaktabPay Pay { get; set; }
    }
}
