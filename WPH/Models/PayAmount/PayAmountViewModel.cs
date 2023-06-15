using System;

namespace WPH.Models.PayAmount
{
    public class PayAmountViewModel
    {
        public Guid Guid { get; set; }
        public Guid PayId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public int? BaseCurrencyId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? DestAmount { get; set; }
    }
}
