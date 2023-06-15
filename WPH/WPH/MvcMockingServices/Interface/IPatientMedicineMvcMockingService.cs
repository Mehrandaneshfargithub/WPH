using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PatientMedicine;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientMedicineMvcMockingService
    {
        List<PatientMedicineRecordViewModel> GetAllMedicineRecordForPatient(Guid id);
        void AddMedicineToPatient(List<PatientMedicineRecordViewModel> diseases);
        void RemoveMedicineFromPatient(Guid[] medicineId, Guid patientId);
        List<PatientMedicineRecordViewModel> GetAllMedicineRecordForPatientGrid(Guid id);
    }
}
