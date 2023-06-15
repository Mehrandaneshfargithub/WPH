using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnSaleInvoiceDetail
{
    public class ReturnSaleInvoiceDetailSelectViewModel : IndexViewModel
    {
         CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? RemainingNum { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public string BujNumber { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SalePrice { get; set; }
        public string Consideration { get; set; }
        public string Currency { get; set; }

        public string InvoiceDateTxt => InvoiceDate?.ToString("dd/MM/yyyy", cultures);
        public string SaleCurrency => $"{Currency} {SalePrice.GetValueOrDefault(0):#,##}";
        public decimal? Total => Num.GetValueOrDefault(0) * SalePrice.GetValueOrDefault(0);
        public decimal? TotalAfterDiscount => Total - Discount.GetValueOrDefault(0);

    }
}
