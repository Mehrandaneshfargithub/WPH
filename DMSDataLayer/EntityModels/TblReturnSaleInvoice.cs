using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReturnSaleInvoice
    {
        public TblReturnSaleInvoice()
        {
            TblReturnSaleInvoiceDetails = new HashSet<TblReturnSaleInvoiceDetail>();
        }

        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public decimal? ReturnDiscount { get; set; }
        public int? UserId { get; set; }
        public int? ReasonId { get; set; }
        public bool? SalePriceChanged { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool? IsDollar { get; set; }
        public bool IsLike { get; set; }
        public bool? OldFactor { get; set; }
        public int? MandoobId { get; set; }
        public int? ModifiedUserId { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblCustomer Customer { get; set; }
        public virtual TblUser User { get; set; }
        public virtual ICollection<TblReturnSaleInvoiceDetail> TblReturnSaleInvoiceDetails { get; set; }
    }
}
