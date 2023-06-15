using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.GroupAnalysis;
using WPH.Models.Reception;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels;

namespace WPH.Models.PatientReceptionAnalysis
{
    public class PatientReceptionAnalysisViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public Guid ReceptionId { get; set; }
        
        public string Name { get; set; }
        public string Code { get; set; }
        public string AnalysisTypeName { get; set; }
        public string AmountCurrencyName { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountCurrencyId { get; set; }
        public decimal? Discount { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? AnalysisItemId { get; set; }
        public Guid? GroupAnalysisId { get; set; }

        public virtual AnalysisViewModel Analysis { get; set; }
        public virtual AnalysisItemViewModel AnalysisItem { get; set; }
        public virtual BaseInfoGeneralViewModel AmountCurrency { get; set; }
        public virtual GroupAnalysisViewModel GroupAnalysis { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }


    }
}