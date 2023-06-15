using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnPurchaseInvoice
{
    public class ReturnPurchaseInvoiceTotalPriceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public bool Purchase { get; set; }
        public string CurrencyName { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? PriceAfterDiscount => TotalPrice - TotalDiscount;
    }
}
