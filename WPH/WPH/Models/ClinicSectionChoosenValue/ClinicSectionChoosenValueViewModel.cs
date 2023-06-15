using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.PatientVariable;

namespace WPH.Models.CustomDataModels.ClinicSectionChoosenValue
{
    public class ClinicSectionChoosenValueViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public Nullable<System.Guid> ClinicSectionId { get; set; }
        public Nullable<int> PatientVariableId { get; set; }
        public string PatientVariableVariableName { get; set; }
        public string PatientVariableAbbreviation { get; set; }
        public string PatientVariableVariableType { get; set; }
        public string VariableDisplayName { get; set; }
        public string VariableStatusName { get; set; }
        public Nullable<int> VariableDisplayId { get; set; }
        public bool ShowInVisit { get; set; }
        public bool ShowInPatient { get; set; }
        public Nullable<int> VariableStatusId { get; set; }
        public bool VariableChangePerVisit { get; set; }
        public Nullable<int> Priority { get; set; }
        public bool FillBySecretary { get; set; }
        public string Value { get; set; }
        public string VariableNameForView { get; set; }
        public Nullable<System.Guid> PatientVariableValueGuid { get; set; }
        public Nullable<DateTime> VariableDate { get; set; }


        public virtual BaseInfoGeneralViewModel VariableDisplay { get; set; }
        public virtual BaseInfoGeneralViewModel VariableStatus { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual PatientVariableViewModel PatientVariable { get; set; }
    }
}