using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        IEnumerable<Patient> GetAllPatientS(bool forGrid, Guid? clinicSectionId);
        string GetLatestPatientFileNum(Guid clinicSectionId, Guid clinicId);
        Patient GetPatient(Guid patientId);
        Patient GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId);
        Patient GetPatientByName(string name, Guid ClinicSectionId);
        IEnumerable<Patient> GetPatientJustNameAndGuid(Guid clinicSectionId);
        IEnumerable<Patient> GetAllPatientForFilter(Guid clinicSectionId);
        IEnumerable<Patient> GetAllClinicPatients(Guid clinicSectionId);
    }

}
