using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblJardSetting
    {
        public int Id { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? OldJard { get; set; }
        public bool? NewJard { get; set; }
        public bool? UseLatestPurchasePrice { get; set; }
        public bool? UseLatestSellingPrice { get; set; }
        public bool? IsHistory { get; set; }
        public string FormName { get; set; }
        public bool? IsExpireDate { get; set; }
        public bool? UseLatestExpireDate { get; set; }
        public bool? IsBuj { get; set; }
        public bool? UseLatestBujNumber { get; set; }
        public bool? BujFirst { get; set; }
        public bool? EnableJardInToolsMenu { get; set; }
        public bool? IsSensitive { get; set; }
        public bool? CanEnterMedicineNumGreaterThanStock { get; set; }
        public bool? IsPurchasePrice { get; set; }
        public bool? IsSalePrice { get; set; }
        public bool? IsConsideration { get; set; }
        public bool? UseLatestConsideration { get; set; }
    }
}
