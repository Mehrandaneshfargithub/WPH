using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPurchaseInvoice
    {
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int? SupplierId { get; set; }
        public string Desc { get; set; }
        public string MainInvoiceNum { get; set; }
        public int? CreatedUserId { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsDollar { get; set; }
        public int ModifiedUserId { get; set; }
        public decimal? InvoiceRemaining { get; set; }
        public decimal? Cost { get; set; }
        public decimal TotalPrice { get; set; }
        public bool? OldFactor { get; set; }
        public int? DaysWaitForReceiveMoney { get; set; }
        public int? CostId { get; set; }
        public int? WarehouseId { get; set; }

        public virtual TblUser CreatedUser { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
        public virtual TblSupplier Supplier { get; set; }
    }
}
