using WPH.Models.CustomDataModels;

namespace WPH.Models.DamageDetail
{
    public class DamageDetailHistoryViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }

        public string PurchaseCurrency => $"{CurrencyName} {Price.GetValueOrDefault(0):#,##}";
        public decimal? Total => Num.GetValueOrDefault(0) * Price.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => Total - Discount.GetValueOrDefault(0);
    }
}
