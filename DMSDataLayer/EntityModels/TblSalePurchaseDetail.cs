using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSalePurchaseDetail
    {
        public TblSalePurchaseDetail()
        {
            TblReturnSaleInvoiceDetails = new HashSet<TblReturnSaleInvoiceDetail>();
        }

        public int Id { get; set; }
        public int SaleInvoiceDetailId { get; set; }
        public int PurchaseInvoiceDetailId { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }

        public virtual TblPurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<TblReturnSaleInvoiceDetail> TblReturnSaleInvoiceDetails { get; set; }
    }
}
