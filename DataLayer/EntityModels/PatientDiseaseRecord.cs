using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PatientDiseaseRecord
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? Patientid { get; set; }
        public Guid? DiseaseId { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
