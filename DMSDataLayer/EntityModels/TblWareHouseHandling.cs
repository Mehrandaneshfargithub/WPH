using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWareHouseHandling
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public string DamagedNum { get; set; }
        public string PurchaseInvoiceNum { get; set; }
        public bool? OldWareHouseHandling { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? ExpireDateSensitive { get; set; }
        public bool? BujNumberSensitive { get; set; }
        public bool? PurchasePriceSensitive { get; set; }
        public bool? FreeNumberSensitive { get; set; }
    }
}
