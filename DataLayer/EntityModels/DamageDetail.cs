using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class DamageDetail
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? MasterId { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? TransferDetailId { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual Damage Master { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Product Product { get; set; }
        public virtual PurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual TransferDetail TransferDetail { get; set; }
    }
}
