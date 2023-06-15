using System;
using System.Collections.Generic;

namespace DataLayer.EntityModels
{
    public partial class TransferDetail
    {
        public TransferDetail()
        {
            DamageDetails = new HashSet<DamageDetail>();
            InverseSourceTransferDetail = new HashSet<TransferDetail>();
            PurchaseInvoiceDetailSalePrices = new HashSet<PurchaseInvoiceDetailSalePrice>();
            ReceptionServices = new HashSet<ReceptionService>();
            ReturnPurchaseInvoiceDetails = new HashSet<ReturnPurchaseInvoiceDetail>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? MasterId { get; set; }
        public decimal? Num { get; set; }
        public string Consideration { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? DestinationProductId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public Guid? TransferDetailId { get; set; }
        public decimal? RemainingNum { get; set; }
        public Guid? SourcePurchaseInvoiceDetailId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? WholeSellPrice { get; set; }
        public decimal? MiddleSellPrice { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual Product DestinationProduct { get; set; }
        public virtual Transfer Master { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual PurchaseInvoiceDetail SourcePurchaseInvoiceDetail { get; set; }
        public virtual TransferDetail SourceTransferDetail { get; set; }
        public virtual BaseInfoGeneral PurchaseCurrency { get; set; }
        public virtual ICollection<DamageDetail> DamageDetails { get; set; }
        public virtual ICollection<TransferDetail> InverseSourceTransferDetail { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePrices { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
    }
}
