using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.AnalysisResult
{
    public class AnalysisResultViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid AnalysisResultMasterId { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string NormalValue { get; set; }
        public string Unit { get; set; }
        public int Priority { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? GroupAnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public bool ShowChart { get; set; }

        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual AnalysisViewModel Analysis { get; set; }
        public virtual AnalysisItemViewModel AnalysisItem { get; set; }
        public virtual GroupAnalysisViewModel GroupAnalysi { get; set; }
    }
}