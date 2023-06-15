using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Pay
    {
        public Pay()
        {
            PayAmounts = new HashSet<PayAmount>();
            PurchaseInvoicePays = new HashSet<PurchaseInvoicePay>();
            ReturnPurchaseInvoicePays = new HashSet<ReturnPurchaseInvoicePay>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SupplierId { get; set; }
        public DateTime? PayDate { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PayAmount> PayAmounts { get; set; }
        public virtual ICollection<PurchaseInvoicePay> PurchaseInvoicePays { get; set; }
        public virtual ICollection<ReturnPurchaseInvoicePay> ReturnPurchaseInvoicePays { get; set; }
    }
}
