using System;

namespace DataLayer.EntityModels
{
    public class ReturnSaleDetailModel
    {
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
    }
}
