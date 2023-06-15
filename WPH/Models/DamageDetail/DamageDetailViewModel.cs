using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.DamageDetail
{
    public class DamageDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? MasterId { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? TransferDetailId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Num { get; set; }
        public string NumTxt { get; set; }
        public decimal? FreeNum { get; set; }
        public string FreeNumTxt { get; set; }
        public decimal? Price { get; set; }
        public string PriceTxt { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TotalPriceTxt { get; set; }
        public decimal? Discount { get; set; }
        public string DiscountTxt { get; set; }
        public string CurrencyName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string DamageDetailTxt { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public string PurchaseCurrency => $"{CurrencyName} {PriceTxt}";
        public decimal? Total => Num.GetValueOrDefault(0) * Price.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => Total - Discount.GetValueOrDefault(0);
    }
}
