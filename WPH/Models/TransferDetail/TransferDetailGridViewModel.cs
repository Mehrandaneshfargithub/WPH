using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.TransferDetail
{
    public class TransferDetailGridViewModel : IndexViewModel
    {
        CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public decimal? Num { get; set; }
        public string Consideration { get; set; }
        public string ProductName { get; set; }
        public string DestinationProductName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? MiddleSellPrice { get; set; }
        public decimal? WholeSellPrice { get; set; }
        public string CurrencyName { get; set; }
        public string PurchasePriceCurrency => $"{PurchasePrice.GetValueOrDefault(0).ToString("#,0.##", cultures)} {CurrencyName}";
        public string SellingPriceCurrency => $"{SellingPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)} {CurrencyName}";
        public string MiddleSellPriceCurrency => $"{MiddleSellPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)} {CurrencyName}";
        public string WholeSellPriceCurrency => $"{WholeSellPrice.GetValueOrDefault(0).ToString("#,0.##", cultures)} {CurrencyName}";
    }
}
