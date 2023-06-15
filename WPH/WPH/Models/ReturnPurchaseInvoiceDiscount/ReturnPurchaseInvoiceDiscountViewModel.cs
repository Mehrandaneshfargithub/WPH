using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnPurchaseInvoiceDiscount
{
    public class ReturnPurchaseInvoiceDiscountViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? ReturnPurchaseInvoiceId { get; set; }
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
