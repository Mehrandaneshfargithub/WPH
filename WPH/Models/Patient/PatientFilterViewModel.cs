using System;

namespace WPH.Models.Patient
{
    public class PatientFilterViewModel
    {
        public Guid Guid { get; set; }
        public string NameAndFileNum { get; set; }
        public string PhoneNumberAndName { get; set; }
        public string FormNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Name { get; set; }
        public int? GenderId { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }
        public string FileNum { get; set; }
        public string MotherName { get; set; }
        public string AddressName { get; set; }
        public Guid? AddressId { get; set; }
        public string FatherJobName { get; set; }
        public Guid? FatherJobId { get; set; }
        public string MotherJobName { get; set; }
        public Guid? MotherJobId { get; set; }
        public string Email { get; set; }
        public string Explanation { get; set; }
    }
}
