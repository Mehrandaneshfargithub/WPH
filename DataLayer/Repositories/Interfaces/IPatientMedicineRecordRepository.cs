using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientMedicineRecordRepository : IRepository<PatientMedicineRecord>
    {
        IEnumerable<PatientMedicineRecord> GetAllPatientMedicineRecord(Guid patientId);
        IEnumerable<PatientMedicineRecord> GetAllPatientMedicineRecordGrid(Guid patientId);
        IEnumerable<PatientMedicineRecord> RemoveMedicineFromPatient(IEnumerable<Guid> medicineIds, Guid patientId);
    }
}
