using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicinePurchaseFactorList
    {
        public int? MasterId { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public int? DetailId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? Paakat { get; set; }
        public decimal? NumInPaakat { get; set; }
        public decimal? Num { get; set; }
        public decimal? PaakatFreeNum { get; set; }
        public decimal? FreeNum { get; set; }
        public decimal? RemainingNum { get; set; }
        public decimal? RemainingFreeNum { get; set; }
        public decimal? Discount { get; set; }
        public string Consideration { get; set; }
        public string BujNumber { get; set; }
    }
}
