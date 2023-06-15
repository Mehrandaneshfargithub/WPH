using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.SaleInvoice
{
    public class SaleInvoiceFilterViewModel
    {
        public int PeriodId { get; set; }
        public DateTime DateFrom { get; set; }
        public string TxtDateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string TxtDateTo { get; set; }
        public Guid? Customer { get; set; }
        public int? Currency { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }
        public Guid? ProductId { get; set; }
    }
}
