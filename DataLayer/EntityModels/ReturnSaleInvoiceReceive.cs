using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReturnSaleInvoiceReceive
    {
        public Guid Guid { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? ReceiveId { get; set; }
        public bool? FullPay { get; set; }

        public virtual ReturnSaleInvoice Invoice { get; set; }
        public virtual Receive Receive { get; set; }
    }
}
