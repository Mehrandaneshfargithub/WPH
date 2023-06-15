using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductReportResultViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string InvoiceNum { get; set; }
        public string InvoiceDate { get; set; }
        public string MainInvoiceNum { get; set; }
        public string Supplier_Customer { get; set; }
        public string Description { get; set; }
        public string ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public string CurrencyName { get; set; }
        public string SaleCurrencyName { get; set; }
        public decimal Num { get; set; }
        public decimal FreeNum { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Discount { get; set; }
        public string Reason { get; set; }
        public string MoneyConvert { get; set; }

        public decimal Total => Num * Price;
        public decimal TotalSale => Num * SalePrice;
        public string DiscountCurrency => $"{CurrencyName} {Discount.ToString("#,0.##", cultures)}";
        public string TotalCurrency => $"{CurrencyName} {Total.ToString("#,0.##", cultures)}";
        public string TotalSaleCurrency => $"{SaleCurrencyName} {TotalSale.ToString("#,0.##", cultures)}";
        public string TotalAfterDiscount => $"{CurrencyName} {(Total - Discount).ToString("#,0.##", cultures)}";
        public string TotalSaleAfterDiscount => $"{SaleCurrencyName} {(TotalSale - Discount).ToString("#,0.##", cultures)}";
        public string PriceCurrency => $"{CurrencyName} {Price.ToString("#,0.##", cultures)}";
        public string SalePriceCurrency => $"{SaleCurrencyName} {SalePrice.ToString("#,0.##", cultures)}";

    }
}
