using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Receive
    {
        public Receive()
        {
            ReceiveAmounts = new HashSet<ReceiveAmount>();
            ReturnSaleInvoiceReceives = new HashSet<ReturnSaleInvoiceReceive>();
            SaleInvoiceReceives = new HashSet<SaleInvoiceReceive>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? CustomerId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public string InvoiceNum { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
        public virtual ICollection<ReceiveAmount> ReceiveAmounts { get; set; }
        public virtual ICollection<ReturnSaleInvoiceReceive> ReturnSaleInvoiceReceives { get; set; }
        public virtual ICollection<SaleInvoiceReceive> SaleInvoiceReceives { get; set; }
    }
}
