using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Transfer
    {
        public Transfer()
        {
            TransferDetails = new HashSet<TransferDetail>();
        }

        public Guid Guid { get; set; }
        public int InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public Guid? SourceClinicSectionId { get; set; }
        public Guid? DestinationClinicSectionId { get; set; }
        public string Description { get; set; }
        public string ReceiverName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ReceiverUserId { get; set; }
        public DateTime? ReceiverDate { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual ClinicSection DestinationClinicSection { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual User ReceiverUser { get; set; }
        public virtual ClinicSection SourceClinicSection { get; set; }
        public virtual ICollection<TransferDetail> TransferDetails { get; set; }
    }
}
