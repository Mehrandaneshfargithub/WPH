using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReturnSaleInvoice
    {
        public ReturnSaleInvoice()
        {
            ReturnSaleInvoiceDetails = new HashSet<ReturnSaleInvoiceDetail>();
            ReturnSaleInvoiceDiscounts = new HashSet<ReturnSaleInvoiceDiscount>();
            ReturnSaleInvoiceReceives = new HashSet<ReturnSaleInvoiceReceive>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid? CustomerId { get; set; }
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
        public virtual Customer Customer { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDetail> ReturnSaleInvoiceDetails { get; set; }
        public virtual ICollection<ReturnSaleInvoiceDiscount> ReturnSaleInvoiceDiscounts { get; set; }
        public virtual ICollection<ReturnSaleInvoiceReceive> ReturnSaleInvoiceReceives { get; set; }
    }
}
