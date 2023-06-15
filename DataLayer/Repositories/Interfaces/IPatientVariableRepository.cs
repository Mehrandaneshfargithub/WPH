using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientVariableRepository : IRepository<PatientVariable>
    {
        IEnumerable<PatientVariable> GetAllPatientVariables(Guid doctorId);
        IEnumerable<PatientVariable> GetAllVariablesForPatient(Guid doctorId, Guid patientId, Guid receptionId, string DisplayType);
    }
}
