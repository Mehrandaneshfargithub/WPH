using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.Analysis;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.AnalysisItemMinMaxValue;
using WPH.Models.CustomDataModels.AnalysisItemValuesRange;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.AnalysisItem
{
    public class AnalysisItemViewModel: IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string Note { get; set; }
        public string NormalValues { get; set; }
        public decimal? Priority { get; set; }
        public int Index { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountCurrencyId { get; set; }
        public Guid? AnalysisId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Code { get; set; }
        public string CodeHolder { get; set; }
        public string Abbreviation { get; set; }
        public int? ValueTypeId { get; set; }
        public Guid? UnitId { get; set; }
        public bool? IsButton { get; set; }
        public string AmountCurrencyName { get; set; }
        public string BaseInfoGeneralName { get; set; }
        public string UnitName { get; set; }
        public string ValueTypeName { get; set; }
        public string AnalysisName { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public string AnalysisItemValuesRanges_Value { get; set; }
        public bool? ShowChart { get; set; }

        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual AnalysisViewModel AnalysisInfo { get; set; }
        public virtual BaseInfoGeneralViewModel AmountCurrency { get; set; }
        public virtual BaseInfoViewModel BaseInfoUnit { get; set; }
        public virtual BaseInfoGeneralViewModel ValueType { get; set; }
        public virtual AnalysisItemMinMaxValueViewModel AnalysisItemMinMaxValues { get; set; }
        public virtual ICollection<AnalysisItemValuesRangeViewModel> AnalysisItemValuesRanges { get; set; }
        public virtual BaseInfoViewModel Unit { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public List<ClinicSectionSettingValueViewModel> AllDecimalAmount { get; set; }
    }
}