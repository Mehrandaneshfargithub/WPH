using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.DamageDiscount
{
    public class DamageDiscountViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? DamageId { get; set; }
        public decimal? Amount { get; set; }
        public string AmountTxt { get; set; }
        public int? CurrencyId { get; set; }
        public string Description { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string CurrencyName { get; set; }
    }
}
