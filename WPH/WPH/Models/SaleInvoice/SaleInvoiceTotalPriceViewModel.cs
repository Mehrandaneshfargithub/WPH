using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.SaleInvoice
{
    public class SaleInvoiceTotalPriceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public string CurrencyName { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? PriceAfterDiscount => TotalPrice - TotalDiscount;
    }
}
