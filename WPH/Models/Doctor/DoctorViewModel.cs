using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.Doctor
{
    public class DoctorViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? SpecialityId { get; set; }
        public string Name { get; set; }
        public string SpecialityName { get; set; }
        public string MedicalSystemCode { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string NameHolder { get; set; }
        public string PhoneNumberHolder { get; set; }
        public string NameAndSpeciallity => $"{UserName ?? ""} | {SpecialityName ?? ""}";
        public string UserName { get; set; }
        public string LogoAddress { get; set; }
        public string Pass1 { get; set; }
        public Guid? ClinicId { get; set; }
        public string Explanation { get; set; }
        public virtual BaseInfoViewModel Speciality { get; set; }
        public virtual UserInformationViewModel User { get; set; }
        public virtual ICollection<ReserveDetailViewModel> ReserveDetails { get; set; }
    }
}