using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.HumanResourceSalary;

namespace WPH.Models.HumanResource
{
    public class HumanResourceWithBeginEndDate : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string HumanName { get; set; }
        public decimal? FixSalary { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public int? RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
        public string Duty { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public virtual BaseInfoGeneralViewModel Currency { get; set; }
        public virtual UserInformationViewModel Gu { get; set; }
        public virtual BaseInfoGeneralViewModel RoleType { get; set; }
        public List<ClinicSectionViewModel> ClinicSections { get; set; }
        public virtual DoctorViewModel Doctor { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public virtual ICollection<HumanResourceSalaryViewModel> HumanResourceSalaries { get; set; }
    }
}
