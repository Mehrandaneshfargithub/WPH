using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSaleInvoice2
    {
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public decimal? Discount { get; set; }
        public int? CreatedUserId { get; set; }
        public bool? IsDollar { get; set; }
        public int ModifiedUserId { get; set; }
        public bool? Enabled { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? InvoiceProfit { get; set; }
        public bool? IsWholesale { get; set; }
        public bool? OldFactor { get; set; }
        public int? PrintedNum { get; set; }
    }
}
