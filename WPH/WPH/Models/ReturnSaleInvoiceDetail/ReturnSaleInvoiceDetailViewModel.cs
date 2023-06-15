using System;
using System.Globalization;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReturnSaleInvoiceDetail
{
    public class ReturnSaleInvoiceDetailViewModel : IndexViewModel
    {
        readonly CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string ChildrenGuids { get; set; }
        public Guid? MasterId { get; set; }
        public Guid? SaleInvoiceDetailId { get; set; }
        public string SaleInvoiceDetailTxt { get; set; }
        public decimal? Num { get; set; }
        public string NumTxt { get; set; }
        public decimal? FreeNum { get; set; }
        public string FreeNumTxt { get; set; }
        public decimal? Price { get; set; }
        public string PriceTxt { get; set; }
        public decimal? TotalPrice { get; set; }
        public string TotalPriceTxt { get; set; }
        public decimal? Discount { get; set; }
        public string DiscountTxt { get; set; }
        public string ReasonTxt { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ChildrenCount { get; set; }

        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }

        public string SaleCurrency => $"{CurrencyName} {Price.GetValueOrDefault(0).ToString("#,#.##", cultures)}";
        public decimal Total => Num.GetValueOrDefault(0) * Price.GetValueOrDefault(0);
        public string TotalAfterDiscount => $"{CurrencyName} {(Total - Discount.GetValueOrDefault(0)).ToString("#,#.##", cultures)}";

    }
}
