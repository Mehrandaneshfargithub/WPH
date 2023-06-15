using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientVariable;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientVariableMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        bool CheckRepeatedPatientVariableName(Guid userId, string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<PatientVariableViewModel> GetAllPatientVariables(Guid doctorId);
        string UpdatePatientVariable(PatientVariableViewModel PatientVariable);
        string AddPatientVariable(PatientVariableViewModel PatientVariable);
        int? GetPatientVariableIdByName(string patientVariableVariableName);
        PatientVariableViewModel GetPatientVariableById(int id);
        OperationStatus RemovePatientVariable(int id);
        IEnumerable<PatientVariableViewModel> GetAllVariablesForPatient(Guid doctorId, Guid patientId, Guid receptionId, string DisplayType);
        void InsertOrUpdatePatientVariableValue(int variableId, Guid receptionId, Guid PatientId, string Value, string status);
    }
}
