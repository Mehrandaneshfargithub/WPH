using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class GroupAnalysisAnalysis
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? GroupAnalysisId { get; set; }
        public Guid? AnalysisId { get; set; }
        public decimal? Priority { get; set; }

        public virtual Analysis Analysis { get; set; }
        public virtual GroupAnalysis GroupAnalysis { get; set; }
    }
}
