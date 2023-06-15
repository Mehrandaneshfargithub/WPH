using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisItemValuesRange
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public string Value { get; set; }
        public bool? Default { get; set; }

        public virtual AnalysisItem AnalysisItem { get; set; }
    }
}
