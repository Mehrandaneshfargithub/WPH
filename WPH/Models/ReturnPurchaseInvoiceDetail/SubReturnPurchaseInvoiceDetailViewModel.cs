using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.ReturnPurchaseInvoiceDetail
{
    public class SubReturnPurchaseInvoiceDetailViewModel
    {
        public int SubIndex { get; set; }
        public Guid SubGuid { get; set; }
        public string SubNum { get; set; }
        public string SubFreeNum { get; set; }
        public string SubPrice { get; set; }
        public string SubTotalPrice { get; set; }
        public string SubDiscount { get; set; }
        public string SubCurrencyName { get; set; }
        public string SubProductName { get; set; }
        public string SubReasonTxt { get; set; }


        public string SubPurchaseCurrency => $"{SubCurrencyName} {SubPrice}";
        public string SubTotal { get; set; }
        public string SubTotalAfterDiscount { get; set; }
    }
}
