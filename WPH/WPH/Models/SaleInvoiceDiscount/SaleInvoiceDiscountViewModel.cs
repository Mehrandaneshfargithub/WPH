using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.SaleInvoiceDiscount
{
    public class SaleInvoiceDiscountViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public decimal? Amount { get; set; }
        public string AmountTxt { get; set; }
        public int? CurrencyId { get; set; }
        public string Description { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CurrencyName { get; set; }
    }
}
