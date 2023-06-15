using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.PatientDisease;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientDiseaseMvcMockingService
    {
        void AddNewDiseasesForPatient(IEnumerable<PatientDiseaseRecordViewModel> patintDisease);
        void RemoveDiseasesFromPatient(Guid[] DiseaseId, Guid PatientId);
        IEnumerable<PatientDiseaseRecordViewModel> GetAllDiseaseForPatient(Guid PatientId);
        List<PatientDiseaseRecordViewModel> GetAllDiseaseForPatientByType(Guid PatientId, string DiseaseType);

    }
}
