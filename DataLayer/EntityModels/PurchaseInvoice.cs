using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoice
    {
        public PurchaseInvoice()
        {
            Costs = new HashSet<Cost>();
            PurchaseInvoiceDetails = new HashSet<PurchaseInvoiceDetail>();
            PurchaseInvoiceDiscounts = new HashSet<PurchaseInvoiceDiscount>();
            PurchaseInvoicePays = new HashSet<PurchaseInvoicePay>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid? SupplierId { get; set; }
        public string Description { get; set; }
        public string MainInvoiceNum { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public bool? OldFactor { get; set; }
        public int? TypeId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual BaseInfoGeneral Type { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetails { get; set; }
        public virtual ICollection<PurchaseInvoiceDiscount> PurchaseInvoiceDiscounts { get; set; }
        public virtual ICollection<PurchaseInvoicePay> PurchaseInvoicePays { get; set; }
    }
}
