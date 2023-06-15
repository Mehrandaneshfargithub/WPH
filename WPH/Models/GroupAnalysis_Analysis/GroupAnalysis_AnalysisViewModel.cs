using System;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.GroupAnalysis;

namespace WPH.Models.CustomDataModels.GroupAnalysis_Analysis
{
    public class GroupAnalysis_AnalysisViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? GroupAnalysisId { get; set; }
        public Guid? AnalysisId { get; set; }
        public decimal? Priority { get; set; }
        public virtual AnalysisViewModel Analysis { get; set; }
        public virtual GroupAnalysisViewModel GroupAnalysis { get; set; }
    }
}