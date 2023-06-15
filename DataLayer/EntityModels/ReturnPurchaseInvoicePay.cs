using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReturnPurchaseInvoicePay
    {
        public Guid Guid { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? PayId { get; set; }
        public bool? FullPay { get; set; }

        public virtual ReturnPurchaseInvoice Invoice { get; set; }
        public virtual Pay Pay { get; set; }
    }
}
