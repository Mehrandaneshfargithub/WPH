using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPurchaseInvoiceDetail
    {
        public TblPurchaseInvoiceDetail()
        {
            TblDamagedDetails = new HashSet<TblDamagedDetail>();
            TblReturnPurchaseInvoiceDetails = new HashSet<TblReturnPurchaseInvoiceDetail>();
            TblSalePurchaseDetails = new HashSet<TblSalePurchaseDetail>();
            TblWarehouseTransferringDetails = new HashSet<TblWarehouseTransferringDetail>();
        }

        public int Id { get; set; }
        public int? MasterId { get; set; }
        public int? MedicineId { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Num { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Consideration { get; set; }
        public int? Priority { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Profit { get; set; }
        public string BujNumber { get; set; }
        public decimal? Paakat { get; set; }
        public decimal? NumInPaakat { get; set; }
        public decimal? FreeNum { get; set; }
        public int UserId { get; set; }
        public decimal? NumRemaining { get; set; }
        public decimal? WholesalePrice { get; set; }
        public decimal? WholesalePriceProfit { get; set; }
        public decimal? NumFreeNumRemaining { get; set; }
        public decimal? LeastSellingPrice { get; set; }
        public decimal? LeastSellingPriceProfit { get; set; }
        public decimal? LeastWholesalePrice { get; set; }
        public decimal? LeastWholesalePriceProfit { get; set; }
        public bool? Fahs { get; set; }
        public bool? Blocked { get; set; }
        public string SupplierConsideration { get; set; }
        public DateTime DetailDate { get; set; }
        public decimal? SharikaFreeNum { get; set; }
        public int? WarehouseId { get; set; }
        public decimal? SecondProfit { get; set; }

        public virtual TblMedicine Medicine { get; set; }
        public virtual TblUser User { get; set; }
        public virtual ICollection<TblDamagedDetail> TblDamagedDetails { get; set; }
        public virtual ICollection<TblReturnPurchaseInvoiceDetail> TblReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<TblSalePurchaseDetail> TblSalePurchaseDetails { get; set; }
        public virtual ICollection<TblWarehouseTransferringDetail> TblWarehouseTransferringDetails { get; set; }
    }
}
