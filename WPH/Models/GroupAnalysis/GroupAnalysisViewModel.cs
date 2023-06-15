using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.GroupAnalysis_Analysis;
using WPH.Models.CustomDataModels.GroupAnalysisItem;
using WPH.Models.PatientReceptionAnalysis;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.GroupAnalysis
{
    public class GroupAnalysisViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }
        public decimal Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public decimal? Priority { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public string DiscountCurrencyName { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAmountWithDiscount { get; set; }
        public bool? IsButton { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual BaseInfoGeneralViewModel DiscountCurrency { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
        public virtual ICollection<GroupAnalysisItemViewModel> GroupAnalysisItems { get; set; }
        public virtual ICollection<GroupAnalysis_AnalysisViewModel> GroupAnalysisAnalyses { get; set; }
        public virtual ICollection<PatientReceptionAnalysisViewModel> PatientReceptionAnalysis { get; set; }
        public List<ClinicSectionSettingValueViewModel> AllDecimalAmount { get; set; }
    }
}