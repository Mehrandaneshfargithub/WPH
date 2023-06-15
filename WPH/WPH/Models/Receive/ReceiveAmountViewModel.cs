using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Receive
{
    public class ReceiveAmountViewModel: IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public Guid ReceiveId { get; set; }
        public decimal? Amount { get; set; }
        public string AmountTxt { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? DestAmount { get; set; }
        public decimal? Discount { get; set; }
        public int? BaseCurrencyId { get; set; }
        public string BaseCurrencyName { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTime ReceiveDate { get; set; }

    }
}
