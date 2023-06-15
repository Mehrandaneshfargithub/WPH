using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientReceptionRepository : IRepository<PatientReception>
    {
        Guid AddNewPatientReception(Reception patientReception, bool newDoctor);
        Reception GetPatientReceptionByIdWithDoctor(Guid patientReceptionId);
        Reception GetPatientReceptionById(Guid patientReceptionId);
        Guid UpdatePatientReception(Reception patientReception, bool newPatient, bool newDoctor);
        string GetLatestReceptionInvoiceNum(Guid clinicSectionId);
        IEnumerable<Reception> GetAllPatientReceptionInvoiceNums(Guid clinicSectionId);
        IEnumerable<Reception> GetAllPatientReceptionPatients(Guid clinicSectionId);
        Reception GetPatientReceptionByIdForAnalysisResult(Guid patientReceptionId);
        Reception GetPatientReceptionByIdForReport(Guid patienReceptionId);
        IEnumerable<PatientImage> RemovePatientReceptionWithReceives(Guid patientReceptionId);
    }
}
