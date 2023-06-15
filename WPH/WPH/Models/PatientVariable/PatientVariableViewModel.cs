using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;
using WPH.Models.CustomDataModels.PatientVariableValue;

namespace WPH.Models.CustomDataModels.PatientVariable
{
    public class PatientVariableViewModel: IndexViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string VariableName { get; set; }
        public string VariableNameHolder { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string VariableDisplay { get; set; }
        public string VariableStatusId { get; set; }
        public bool FillBySecreter { get; set; }
        public bool Value { get; set; }
        public Guid PatientId { get; set; }
        public string VariableTypeName { get; set; }
        public int VariableTypeId { get; set; }
        public string VariableUnit { get; set; }
        public Guid? DoctorId { get; set; }

        public int? VariableDisplayId { get; set; }
        public string VariableDisplayName { get; set; }
        public string VariableStatusName { get; set; }
        public int? Priority { get; set; }
        public List<List<PatientVariablesValueViewModel>> AllVariableValues { get; set; }

        public virtual ICollection<ClinicSectionChoosenValueViewModel> ClinicSectionChoosenValues { get; set; }
        public virtual ICollection<PatientVariablesValueViewModel> PatientVariablesValues { get; set; }
        
    }
}