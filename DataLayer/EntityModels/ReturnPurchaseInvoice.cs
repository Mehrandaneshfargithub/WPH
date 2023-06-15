using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReturnPurchaseInvoice
    {
        public ReturnPurchaseInvoice()
        {
            ReturnPurchaseInvoiceDetails = new HashSet<ReturnPurchaseInvoiceDetail>();
            ReturnPurchaseInvoiceDiscounts = new HashSet<ReturnPurchaseInvoiceDiscount>();
            ReturnPurchaseInvoicePays = new HashSet<ReturnPurchaseInvoicePay>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid? SupplierId { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public bool? OldFactor { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDetail> ReturnPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<ReturnPurchaseInvoiceDiscount> ReturnPurchaseInvoiceDiscounts { get; set; }
        public virtual ICollection<ReturnPurchaseInvoicePay> ReturnPurchaseInvoicePays { get; set; }
    }
}
