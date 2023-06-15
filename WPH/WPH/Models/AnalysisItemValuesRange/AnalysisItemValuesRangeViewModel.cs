using System;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;

namespace WPH.Models.CustomDataModels.AnalysisItemValuesRange
{
    public class AnalysisItemValuesRangeViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Nullable<System.Guid> AnalysisItemId { get; set; }
        public string Value { get; set; }
        public Nullable<bool> Default { get; set; }
        public string BaseInfoGeneralUnitName { get; set; }
        public string AnalysisItemName { get; set; }
        //public virtual AnalysisItemViewModel AnalysisItem { get; set; }
    }
}