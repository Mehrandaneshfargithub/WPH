using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisAnalysisItem
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public int? Priority { get; set; }

        public virtual Analysis Analysis { get; set; }
        public virtual AnalysisItem AnalysisItem { get; set; }
    }
}
