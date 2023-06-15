using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientVariable
    {
        public PatientVariable()
        {
            ClinicSectionChoosenValues = new HashSet<ClinicSectionChoosenValue>();
            PatientVariablesValues = new HashSet<PatientVariablesValue>();
        }

        public int Id { get; set; }
        public string VariableName { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public int? VariableTypeId { get; set; }
        public string VariableUnit { get; set; }
        public Guid? DoctorId { get; set; }
        public int? VariableDisplayId { get; set; }
        public int? VariableStatusId { get; set; }
        public int? Priority { get; set; }

        public virtual BaseInfoGeneral VariableType { get; set; }
        public virtual BaseInfoGeneral VariableDisplay { get; set; }
        public virtual BaseInfoGeneral VariableStatus { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<ClinicSectionChoosenValue> ClinicSectionChoosenValues { get; set; }
        public virtual ICollection<PatientVariablesValue> PatientVariablesValues { get; set; }
    }
}
