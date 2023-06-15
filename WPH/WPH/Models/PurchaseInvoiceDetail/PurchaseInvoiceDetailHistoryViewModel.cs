using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.PurchaseInvoiceDetail
{
    public class PurchaseInvoiceDetailHistoryViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? Date { get; set; }
        public string Supplier { get; set; }
        public string MainInvoiceNum { get; set; }
        public string BujNumber { get; set; }
        public string CurrencyName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }

        public decimal? Discount { get; set; }
        public string Consideration { get; set; }
         
        public string PurchaseCurrency => $"{CurrencyName} {PurchasePrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public string SellingPriceCurrency => $"{CurrencyName} {SellingPrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public decimal? Total => Num.GetValueOrDefault(0) * PurchasePrice.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => Total - Discount.GetValueOrDefault(0);

    }
}
