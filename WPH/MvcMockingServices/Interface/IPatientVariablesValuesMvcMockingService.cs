using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientVariableValue;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientVariablesValuesMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<PatientVariablesValueViewModel> GetAllPatientVariablesValueBasedOnPatientId(Guid? patientId);
        IEnumerable<PatientVariablesValueViewModel> GetAllPatientVariablesValueBasedOnVisitId(Guid VisitId);
        void UpdatePatientVariablesValue(PatientVariablesValueViewModel PatientVariablesValue);
        Guid AddPatientVariablesValue(PatientVariablesValueViewModel PatientVariablesValue);
        PatientVariablesValueViewModel GetPatientVariablesValueBasedOnGuid(Guid Guid);
        void RemoveRange(IEnumerable<PatientVariablesValueViewModel> mustRemove);
        OperationStatus Remove(Guid Guid);
        void UpdatePatientVariablesValueBasedOnGuid(Guid Guid, string Value);
        IEnumerable<PatientVariablesValueViewModel> GetAllPatientSpeceficVariable(Guid receptionId, string variableName);
        void AddPatientVariablesValueRange(IEnumerable<PatientVariablesValueViewModel> allValues);
        IEnumerable<PatientVariablesValueViewModel> GetAllReceptionVariable(Guid receptionId);
    }
}
