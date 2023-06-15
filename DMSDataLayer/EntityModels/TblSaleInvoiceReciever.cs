using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoiceReciever
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int RecieverId { get; set; }
        public bool? FullPay { get; set; }
    }
}
