using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.AnalysisItem;

namespace WPH.Models.CustomDataModels.Analysis_AnalysisItem
{
    public class Analysis_AnalysisItemViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public int Priority { get; set; }
        public virtual AnalysisViewModel Analysis { get; set; }
        public virtual AnalysisItemViewModel AnalysisItem { get; set; }
    }
}