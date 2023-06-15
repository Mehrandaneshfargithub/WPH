using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReturnPurchaseInvoice
    {
        public TblReturnPurchaseInvoice()
        {
            TblReturnPurchaseInvoiceDetails = new HashSet<TblReturnPurchaseInvoiceDetail>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public string Description { get; set; }
        public decimal? ReturnDiscount { get; set; }
        public int? UserId { get; set; }
        public int? ReasonId { get; set; }
        public int? SecondSupplierId { get; set; }
        public bool? PurchasePriceChanged { get; set; }
        public bool? IsDollar { get; set; }
        public bool IsLike { get; set; }
        public bool? Ended { get; set; }
        public bool? OldFactor { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblBaseInfo Reason { get; set; }
        public virtual TblSupplier Supplier { get; set; }
        public virtual TblUser User { get; set; }
        public virtual ICollection<TblReturnPurchaseInvoiceDetail> TblReturnPurchaseInvoiceDetails { get; set; }
    }
}
