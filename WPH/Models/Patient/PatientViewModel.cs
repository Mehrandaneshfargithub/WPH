using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.PatientDisease;
using WPH.Models.CustomDataModels.PatientMedicine;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.Patient
{
    public class PatientViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        
        public string FileNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string DateOfBirthTxt { get; set; }
        public string DateOfBirthYear { get; set; }
        public string DateOfBirthMonth { get; set; }
        public string DateOfBirthDay { get; set; }
        public int? Age { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? BloodTypeId { get; set; }
        public string BloodType { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Pass1 { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string PhoneNumber { get; set; }
        public int? GenderId { get; set; }
        public string UserGenderName { get; set; }
        public bool? Active { get; set; }
        public string NameHolder { get; set; }
        public string PhoneNumberHolder { get; set; }
        public Guid? FatherJobId { get; set; }
        public Guid? MotherJobId { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string FormNumber { get; set; }
        public bool? FileNumChoose { get; set; }
        public string FatherJobName { get; set; }
        public string MotherJobName { get; set; }
        public string AddressName { get; set; }
        public string NameAndFileNum { get; set; }
        public string PhoneNumberAndName { get; set; }
        public string MotherName { get; set; }
        public string IdentityNumber { get; set; }
        public virtual UserInformationViewModel User { get; set; }
        public virtual ICollection<PatientDiseaseRecordViewModel> PatientDiseaseRecords { get; set; }
        public virtual ICollection<PatientMedicineRecordViewModel> PatientMedicineRecords { get; set; }
        public virtual ICollection<ReserveDetailViewModel> ReserveDetails { get; set; }
        public virtual BaseInfoGeneralViewModel BaseInfoGeneral { get; set; }
        public virtual BaseInfoViewModel FatherJob { get; set; }
        public virtual BaseInfoViewModel MotherJob { get; set; }
        public virtual BaseInfoViewModel Address { get; set; }
    }
}