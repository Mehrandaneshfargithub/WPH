using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class GroupAnalysisItem
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? GroupAnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public decimal? Priority { get; set; }

        public virtual AnalysisItem AnalysisItem { get; set; }
        public virtual GroupAnalysis GroupAnalysis { get; set; }
    }
}
