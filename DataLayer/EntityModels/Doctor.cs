using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Doctor
    {
        public Doctor()
        {
            Children = new HashSet<Child>();
            PatientReceptions = new HashSet<PatientReception>();
            PatientVariables = new HashSet<PatientVariable>();
            ReceptionDoctors = new HashSet<ReceptionDoctor>();
            ReserveDetails = new HashSet<ReserveDetail>();
            SurgeryDoctors = new HashSet<SurgeryDoctor>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string MedicalSystemCode { get; set; }
        public Guid? SpecialityId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string LogoAddress { get; set; }
        public string Explanation { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User User { get; set; }
        public virtual BaseInfo Speciality { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ICollection<PatientReception> PatientReceptions { get; set; }
        public virtual ICollection<PatientVariable> PatientVariables { get; set; }
        public virtual ICollection<ReceptionDoctor> ReceptionDoctors { get; set; }
        public virtual ICollection<ReserveDetail> ReserveDetails { get; set; }
        public virtual ICollection<SurgeryDoctor> SurgeryDoctors { get; set; }
    }
}
