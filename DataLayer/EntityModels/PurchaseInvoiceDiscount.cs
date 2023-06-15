using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoiceDiscount
    {
        public Guid Guid { get; set; }
        public Guid? PurchaseInvoiceId { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public string Description { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User CreateUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
    }
}
