using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReturnSaleInvoiceReceiver
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int? ReceiverId { get; set; }
        public bool? FullPay { get; set; }
    }
}
