using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionService
    {
        public ReceptionService()
        {
            ReceptionServiceReceiveds = new HashSet<ReceptionServiceReceived>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Number { get; set; }
        public int? StatusId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ServiceDate { get; set; }
        public Guid? ProductId { get; set; }
        public int? ProductIdDMS { get; set; }
        public string Explanation { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? TransferDetailId { get; set; }


        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral DiscountCurrency { get; set; }
        public virtual Product Product { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual Service Service { get; set; }
        public virtual BaseInfoGeneral Status { get; set; }
        public virtual PurchaseInvoiceDetail PurchaseInvoiceDetail { get; set; }
        public virtual TransferDetail TransferDetail { get; set; }
        public virtual ICollection<ReceptionServiceReceived> ReceptionServiceReceiveds { get; set; }
    }
}
