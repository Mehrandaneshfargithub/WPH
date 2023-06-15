using System;
using System.Globalization;

namespace WPH.Models.ReturnSaleInvoiceDetail
{
    public class SubReturnSaleInvoiceDetailViewModel
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


        public string SubSaleCurrency => $"{SubCurrencyName} {SubPrice}";
        public string SubTotal { get; set; }
        public string SubTotalAfterDiscount { get; set; }
    }
}
