using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientVariablesValueRepository : IRepository<PatientVariablesValue>
    {
        List<PatientVariablesValue> GetAllPatientVariablesValueBasedOnVisitId(Guid visitId);
        void UpdatePatientVariablesValueBasedOnGuid(Guid Guid, string value);
        List<PatientVariablesValue> GetAllPatientSpeceficVariable(Guid receptionId, string variableName);
        IEnumerable<PatientVariablesValue> GetAllReceptionVariable(Guid receptionId);
    }
}
