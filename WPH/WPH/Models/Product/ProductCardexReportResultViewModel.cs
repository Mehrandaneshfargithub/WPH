using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductCardexReportResultViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public string Type { get; set; }
        public string InvoiceDate { get; set; }
        public string Description { get; set; }
        public decimal InNum { get; set; }
        public decimal InFreeNum { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal OutNum { get; set; }
        public decimal OutFreeNum { get; set; }
        public decimal SalePrice { get; set; }
        public string ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public string CurrencyName { get; set; }
        public decimal RemainingNum { get; set; }

        public decimal TotalIn => InNum * PurchasePrice;
        public decimal TotalOut => OutNum * SalePrice;
        public string TotalInCurrency => TotalIn == 0 ? "0" : $"{CurrencyName} {TotalIn.ToString("#,0.##", cultures)}";
        public string TotalOutCurrency => TotalOut == 0 ? "0" : $"{CurrencyName} {TotalOut.ToString("#,0.##", cultures)}";
        public string PurchasePriceCurrency => $"{CurrencyName} {PurchasePrice.ToString("#,0.##", cultures)}";
        public string SalePriceCurrency => $"{CurrencyName} {SalePrice.ToString("#,0.##", cultures)}";
    }
}
