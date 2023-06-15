using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.PurchaseInvoiceDetail
{
    public class PurchaseInvoiceDetailViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }

        public Guid? MasterId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? MoneyConvertId { get; set; }
        public string ProductName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string ExpireDateTxt { get; set; }
        public decimal? Num { get; set; }
        public string NumTxt { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string PurchasePriceTxt { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? OldSalePrice { get; set; }
        public decimal? FirstPrice { get; set; }
        public string SellingPriceTxt { get; set; }
        public string SaleType { get; set; }
        public int? SellingCurrencyId { get; set; }
        public string Consideration { get; set; }
        public string PurchaseType { get; set; }

        public decimal? Discount { get; set; }
        public string DiscountTxt { get; set; }

        public decimal? FreeNum { get; set; }
        public string FreeNumTxt { get; set; }
        public decimal? WholePurchasePrice { get; set; }
        public string WholePurchasePriceTxt { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }

        public int? CurrencyId { get; set; }
        public int? FirstCurrencyId { get; set; }

        public decimal? WholeSellPrice { get; set; }
        public string WholeSellPriceTxt { get; set; }

        public decimal? MiddleSellPrice { get; set; }
        public string MiddleSellPriceTxt { get; set; }

        public string CurrencyName { get; set; }

        public string BujNumber { get; set; }
        public string Stock { get; set; }
        public string TotalStock { get; set; }


        public string PurchaseCurrency => $"{CurrencyName} {PurchasePrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public string SellingPriceCurrency => $"{CurrencyName} {SellingPrice.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public decimal Total => Num.GetValueOrDefault(0) * PurchasePrice.GetValueOrDefault(0);
        public string TotalAfterDiscount => $"{CurrencyName} {(Total - Discount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}";

    }
}
