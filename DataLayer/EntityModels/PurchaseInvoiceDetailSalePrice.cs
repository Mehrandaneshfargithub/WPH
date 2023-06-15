using System;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoiceDetailSalePrice
    {
        public Guid Guid { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public int? TypeId { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? TransferDetailId { get; set; }

        public virtual User CreateUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual MoneyConvert MoneyConvert { get; set; }
        public virtual PurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual TransferDetail TransferDetail { get; set; }
        public virtual BaseInfoGeneral Type { get; set; }
    }
}
