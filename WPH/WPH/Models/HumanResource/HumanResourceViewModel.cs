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
    public class HumanResourceViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string HumanName { get; set; }
        public decimal? FixSalary { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public int? RoleTypeId { get; set; }
        public int? RoleTypeIdHolder { get; set; }
        public string RoleTypeName { get; set; } 
        public string Duty { get; set; }
        public decimal? ExtraSalaryPh { get; set; }
        public decimal? MinWorkTime { get; set; }
        public bool IsUser { get; set; }

        public virtual UserInformationViewModel Gu { get; set; }
        public virtual DoctorViewModel Doctor { get; set; }
    }
}
