using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AnalysisItemMinMaxValue
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public decimal? MMinValue { get; set; }
        public decimal? MMaxValue { get; set; }
        public decimal? FMinValue { get; set; }
        public decimal? FMaxValue { get; set; }
        public decimal? CMinValue { get; set; }
        public decimal? CMaxValue { get; set; }
        public decimal? BMinValue { get; set; }
        public decimal? BMaxValue { get; set; }

        public virtual AnalysisItem AnalysisItem { get; set; }
    }
}
