using System;
using WPH.Models.CustomDataModels.AnalysisItem;

namespace WPH.Models.CustomDataModels.AnalysisItemMinMaxValue
{
    public class AnalysisItemMinMaxValueViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Nullable<System.Guid> AnalysisItemId { get; set; }
        public decimal? MMinValue { get; set; }
        public decimal? MMaxValue { get; set; }
        public decimal? FMinValue { get; set; }
        public decimal? FMaxValue { get; set; }
        public decimal? CMinValue { get; set; }
        public decimal? CMaxValue { get; set; }
        public decimal? BMinValue { get; set; }
        public decimal? BMaxValue { get; set; }
        public string AnalysisItemName { get; set; }
        //public virtual AnalysisItemViewModel AnalysisItem { get; set; }
    }
}