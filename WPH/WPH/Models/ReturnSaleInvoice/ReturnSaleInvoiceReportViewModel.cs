using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.ReturnSaleInvoice
{
    public class ReturnSaleInvoiceReportViewModel
    {
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string Product { get; set; }
        public string Customer { get; set; }
        public string Num => TempNum.ToString("N0");
        public string SalePrice => TempSalePrice.ToString("N0");
        public string WholeSalePrice => TempWholeSalePrice.ToString("N0");
        public string Discount => TempDiscount.ToString("N0");
        public string WholeDiscount => TempWholeDiscount.ToString("N0");
        public string Currency { get; set; }

        public decimal TempNum { get; set; }
        public decimal TempSalePrice { get; set; }
        public decimal TempDiscount { get; set; }
        public decimal TempWholeSalePrice { get; set; }
        public decimal TempWholeDiscount { get; set; }
    }
}
