using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.PatientVariable;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.PatientVariableValue
{
    public class PatientVariablesValueViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public Nullable<System.Guid> ClinicSectionId { get; set; }
        public Nullable<System.Guid> VisitId { get; set; }
        public Nullable<System.Guid> PatientId { get; set; }
        public Nullable<System.DateTime> VariableInsertedDate { get; set; }
        public string VariableInsertedDateDay { get; set; }
        public string VariableInsertedDateMonth { get; set; }
        public string VariableInsertedDateYear { get; set; }
        public string VariableInsertedDateHour { get; set; }
        public string VariableInsertedDateMin { get; set; }
        public Nullable<int> PatientVariableId { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }
        public Guid? ReceptionId { get; set; }
        public string PatientVariableVariableName { get; set; }
        public string PatientVariableVariableUnit { get; set; }
        public string PatientVariableDescription { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }

        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public virtual PatientVariableViewModel PatientVariable { get; set; }
        public virtual VisitViewModel Visit { get; set; }
    }
}