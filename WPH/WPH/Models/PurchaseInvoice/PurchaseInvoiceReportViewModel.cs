using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.PurchaseInvoice
{
    public class PurchaseInvoiceReportViewModel
    {
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string Product { get; set; }
        public string Supplier { get; set; }
        public string Num => TempNum.ToString("N0");
        public string PurchasePrice => TempPurchasePrice.ToString("N0");
        public string WholePurchasePrice => TempWholePurchasePrice.ToString("N0");
        public string Discount => TempDiscount.ToString("N0");
        public string WholeDiscount => TempWholeDiscount.ToString("N0");
        public string Currency { get; set; }

        public decimal TempNum { get; set; }
        public decimal TempPurchasePrice { get; set; }
        public decimal TempDiscount { get; set; }
        public decimal TempWholePurchasePrice { get; set; }
        public decimal TempWholeDiscount { get; set; }
    }
}
