using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.GroupAnalysisItem
{
    public class GroupAnalysisItemViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Nullable<System.Guid> GroupAnalysisId { get; set; }
        public Nullable<System.Guid> AnalysisItemId { get; set; }
        public Nullable<decimal> Priority { get; set; }
        public virtual AnalysisItemViewModel AnalysisItem { get; set; }
        public virtual GroupAnalysisViewModel GroupAnalysis { get; set; }
    }
}