using System;
using System.Collections.Generic;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoiceDetail
    {
        public PurchaseInvoiceDetail()
        {
            DamageDetails = new HashSet<DamageDetail>();
            PurchaseInvoiceDetailSalePrices = new HashSet<PurchaseInvoiceDetailSalePrice>();
            ReceptionServices = new HashSet<ReceptionService>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
            TransferDetails = new HashSet<TransferDetail>();
            ReturnPurchaseInvoiceDetails = new HashSet<ReturnPurchaseInvoiceDetail>();
            TransferDetailSourcePurchaseInvoiceDetails = new HashSet<TransferDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? MasterId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Num { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string Consideration { get; set; }
        public int? Priority { get; set; }
        public decimal? Discount { get; set; }
        public decimal? FreeNum { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? RemainingNum { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? WholeSellPrice { get; set; }
        public decimal? MiddleSellPrice { get; set; }
        public string BujNumber { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual PurchaseInvoice Master { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<DamageDetail> DamageDetails { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePrices { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public virtual ICollection<TransferDetail> TransferDetails { get; set; }
        public virtual ICollection<TransferDetail> TransferDetailSourcePurchaseInvoiceDetails { get; set; }
    }
}
