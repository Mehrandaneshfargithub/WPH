using System.Globalization;

namespace WPH.Models.CustomerAccount
{
    public class CustomerAccountReportDetailViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public string Guid { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public string EnInvoiceType { get; set; }
        public string InvoiceType { get; set; }
        public string Description { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string CurrencyName { get; set; }
        public string ProducerName { get; set; }
        public string ExpiryDate { get; set; }
        public decimal TempNum { get; set; }
        public string Num => TempNum.ToString("#,0.##", cultures);
        public decimal TempFreeNum { get; set; }
        public string FreeNum => TempFreeNum.ToString("#,0.##", cultures);
        public decimal TempSalePrice { get; set; }
        public string SalePrice => $"{CurrencyName} {TempSalePrice.ToString("#,0.##", cultures)}";
        public decimal TempDiscount { get; set; }
        public string Discount => TempDiscount.ToString("#,0.##", cultures);
        public string TotalDiscount { get; set; }
        public string VTotal => $"{CurrencyName} {((TempNum * TempSalePrice) - TempDiscount).ToString("#,0.##", cultures)}";
        public string TotalPrice { get; set; }
    }
}
