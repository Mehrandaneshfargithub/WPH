using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMandoobHistory
    {
        public int Id { get; set; }
        public int? MainSaleInvoiceDetailId { get; set; }
        public int? MandoobSaleInvoiceDetailId { get; set; }

        public virtual TblSaleInvoiceDetail MainSaleInvoiceDetail { get; set; }
        public virtual TblSaleInvoiceDetail MandoobSaleInvoiceDetail { get; set; }
    }
}
