using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Patient
    {
        public Patient()
        {
            PatientDiseaseRecords = new HashSet<PatientDiseaseRecord>();
            PatientImages = new HashSet<PatientImage>();
            PatientMedicineRecords = new HashSet<PatientMedicineRecord>();
            PatientReceptions = new HashSet<PatientReception>();
            PatientVariablesValues = new HashSet<PatientVariablesValue>();
            Receptions = new HashSet<Reception>();
            ReserveDetails = new HashSet<ReserveDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string FileNum { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? BloodTypeId { get; set; }
        public Guid? FatherJobId { get; set; }
        public Guid? MotherJobId { get; set; }
        public string FormNumber { get; set; }
        public Guid? AddressId { get; set; }
        public string MotherName { get; set; }
        public string IdentityNumber { get; set; }

        public virtual BaseInfo Address { get; set; }
        public virtual BaseInfoGeneral BloodType { get; set; }
        public virtual BaseInfo FatherJob { get; set; }
        public virtual User User { get; set; }
        public virtual BaseInfo MotherJob { get; set; }
        public virtual ICollection<PatientDiseaseRecord> PatientDiseaseRecords { get; set; }
        public virtual ICollection<PatientImage> PatientImages { get; set; }
        public virtual ICollection<PatientMedicineRecord> PatientMedicineRecords { get; set; }
        public virtual ICollection<PatientReception> PatientReceptions { get; set; }
        public virtual ICollection<PatientVariablesValue> PatientVariablesValues { get; set; }
        public virtual ICollection<ReserveDetail> ReserveDetails { get; set; }
        public virtual ICollection<Reception> Receptions { get; set; }
    }
}
