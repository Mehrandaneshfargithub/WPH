using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;
namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientDiseaseRecordRepository : IRepository<PatientDiseaseRecord>
    {
        IEnumerable<PatientDiseaseRecord> GetAllPatientDiseaseRecord(Guid PatientId);
        IEnumerable<PatientDiseaseRecord> GetAllPatientDiseaseRecordByType(Guid PatientId, string DiseaseType);
        IEnumerable<PatientDiseaseRecord> RemoveDiseasesFromPatient(IEnumerable<Guid> diseaseIds, Guid PatientId);
    }
}
