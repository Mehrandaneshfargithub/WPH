using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.AnalysisItem;

namespace WPH.Models.CustomDataModels.Analysis
{
    public class AnalysisWithAnalysisItemViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<bool> IsButton { get; set; }
        public List<AnalysisItemJustNameAndGuidViewModel> AnalysisItems { get; set; }

    }
}