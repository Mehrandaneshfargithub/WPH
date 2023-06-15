using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.Analysis_AnalysisItem;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.Analysis
{
    public class AnalysisViewModel: IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string Code { get; set; }
        public string CodeHolder { get; set; }
        public string Abbreviation { get; set; }
        public decimal Discount { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string CreatedUserName { get; set; }
        public string ModifiedUserName { get; set; }
        public string DiscountCurrencyName { get; set; }
        public string GridFormat { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalAmountWithDiscount { get; set; }
        public int? Priority { get; set; }
        public bool? IsButton { get; set; }
        public virtual ICollection<Analysis_AnalysisItemViewModel> Analysis_AnalysisItem { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual BaseInfoGeneralViewModel DiscountCurrency { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }

        public List<ClinicSectionSettingValueViewModel> AllDecimalAmount { get; set; }
    }
}