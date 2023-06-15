using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Models.MaterialStoreroom
{
    public class MaterialProductHistoryViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public decimal? Num { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public string SourceClinicSectionName { get; set; }
        public string DestinationClinicSectionName { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
