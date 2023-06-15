using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoiceDetail
    {
        public SaleInvoiceDetail()
        {
            ReturnSaleInvoiceDetails = new HashSet<ReturnSaleInvoiceDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public decimal? Num { get; set; }
        public decimal? SalePrice { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public string Consideration { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? Discount { get; set; }
        public Guid? TransferDetailId { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? ProductId { get; set; }
        public string BujNumber { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public decimal? RemainingNum { get; set; }
        public decimal? OldSalePrice { get; set; }
        

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual MoneyConvert MoneyConvert { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
        public virtual TransferDetail TransferDetail { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetails { get; set; }
    }
}
