using System;

namespace WPH.Models.SupplierAccount
{
    public class SupplierAccountReportViewModel
    {
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }
        public string EnInvoiceType { get; set; }
        public string InvoiceType { get; set; }
        public string TotalPrice { get; set; }
        public string Discount { get; set; }
        public string TotalAfterDiscount { get; set; }
        public string TempRem { get; set; }
        public string Rem => TempRem.Replace("_", "<br>");
    }
}
