using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisResult
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? GroupAnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public Guid? AnalysisResultMasterId { get; set; }
        public bool? ShowChart { get; set; }

        public virtual Analysis Analysis { get; set; }
        public virtual AnalysisItem AnalysisItem { get; set; }
        public virtual AnalysisResultMaster AnalysisResultMaster { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual GroupAnalysis GroupAnalysis { get; set; }
        public virtual User ModifiedUser { get; set; }
    }
}
