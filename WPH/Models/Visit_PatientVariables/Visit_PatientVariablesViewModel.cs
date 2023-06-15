using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.Visit_PatientVariables
{
    public class Visit_PatientVariablesViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public Nullable<System.Guid> VisitId { get; set; }
        public Nullable<System.DateTime> VariableDate { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public decimal BloodPressureSYS { get; set; }
        public decimal BloodPressureDIA { get; set; }
        public decimal BodyTemperature { get; set; }
        public Nullable<System.Guid> PatientId { get; set; }
        public Nullable<int> Age { get; set; }
        public virtual VisitViewModel Visit { get; set; }
        public virtual PatientViewModel Patient { get; set; }
    }
}