using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.PurchaseInvoiceDiscount;

namespace WPH.Models.PurchaseInvoice
{
    public class PrintPurchaseReportViewModel
    {
        public string Supplier { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string Description { get; set; }
        public IEnumerable<TotalDiscountViewModel> Totals { get; set; }
        public IEnumerable<PrintPurchaseDetailReportViewModel> Details { get; set; }
    }
}
