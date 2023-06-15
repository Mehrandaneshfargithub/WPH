using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.AnalysisItem
{
    public class AnalysisItemJustNameAndGuidViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public bool? IsButton { get; set; }
        public decimal? Priority { get; set; }
    }
}