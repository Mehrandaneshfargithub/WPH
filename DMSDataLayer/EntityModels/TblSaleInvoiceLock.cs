using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoiceLock
    {
        public int Id { get; set; }
        public int SaleInvoiceId { get; set; }
        public bool MainLock { get; set; }
        public bool SubLock { get; set; }
        public string SaleInvoiceNum { get; set; }
    }
}
