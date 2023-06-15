using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ClinicSectionChoosenValue
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? PatientVariableId { get; set; }
        public int? VariableDisplayId { get; set; }
        public int? VariableStatusId { get; set; }
        public int? Priority { get; set; }
        public bool? FillBySecretary { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual PatientVariable PatientVariable { get; set; }
        public virtual BaseInfoGeneral VariableDisplay { get; set; }
        public virtual BaseInfoGeneral VariableStatus { get; set; }
    }
}
