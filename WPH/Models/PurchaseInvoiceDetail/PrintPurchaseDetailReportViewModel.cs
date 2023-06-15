using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.PurchaseInvoiceDetail
{
    public class PrintPurchaseDetailReportViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public string ProductName { get; set; }
        public string Producer { get; set; }
        public string ProductType { get; set; }
        public string ExpireDate { get; set; }
        public string Num { get; set; }
        public string FreeNum { get; set; }
        public string CurrencyName { get; set; }
        public decimal TempPurchasePrice { get; set; }
        public decimal TempDiscount { get; set; }
        public decimal TempTotal { get; set; }

        public string PurchasePrice => $"{CurrencyName} {TempPurchasePrice.ToString("#,0.##", cultures)}";
        public string Total => $"{CurrencyName} {TempTotal.ToString("#,0.##", cultures)}";
        public string Discount => $"{CurrencyName} {TempDiscount.ToString("#,0.##", cultures)}";
        public string TotalAfterDiscount => $"{CurrencyName} {(TempTotal - TempDiscount).ToString("#,0.##", cultures)}";

    }
}
