using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientVariablesValue
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? PatientId { get; set; }
        public DateTime? VariableInsertedDate { get; set; }
        public int? PatientVariableId { get; set; }
        public string Value { get; set; }
        public Guid? ReceptionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual PatientVariable PatientVariable { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
