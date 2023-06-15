using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.Patient;
using WPH.Models.PatientImage;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientMvcMockingService
    {
        string AddPatient(PatientViewModel patientt, Guid clinicSectionId);
        string GetPatientFileNum(Guid clinicSectionId, Guid clinicId);
        string NextInvoiceNum(string str);
        IEnumerable<PatientViewModel> GetAllPatients(bool forGrid, Guid? clinicSectionId);
        Task<List<PatientViewModel>> GetAllPatientsWithCombinedNameAndFileNum(bool forGrid, Guid clinicSectionId, Guid clinicId, int sectionTypeId);
        Task<List<PatientViewModel>> GetAllPatientsWithCombinedNameAndFileNumForReserve(bool forGrid, Guid clinicSectionId, Guid clinicId, int sectionTypeId);
        List<PatientViewModel> GetAllPatientsWithCombinedNameAndPhoneNumber(bool forGrid, Guid clinicId);
        PatientViewModel GetPatient(Guid patientId);
        PatientViewModel GetPatientWithCombinedNameAndPhone(Guid patientId);
        Guid GetPatientIdByName(string name, Guid clinicSectionId);
        void GetModalsViewBags(dynamic viewBag);
        OperationStatus RemovePatient(Guid patientId);
        void UpdatePatient(PatientViewModel patient);
        bool CheckRepeatedNameAndNumber(string name, string phoneNumber, Guid clinicId, bool NewOrUpdate, string oldName = "", string oldNumber = "");
        string getLastPatientFileNumber(Guid clinicSectionId, Guid clinicId);
        string RandomString(int length);
        PatientViewModel GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId);
        PatientViewModel CheckNewPatient(ref bool newPatient, PatientViewModel patient, Guid clinicId, Guid clinicSectionId);
        IEnumerable<PatientViewModel> GetPatientJustNameAndGuid(Guid clinicSectionId);
        Task<List<PatientFilterViewModel>> GetAllPatientForFilter(Guid clinicSectionId);
        IEnumerable<PatientViewModel> GetAllClinicPatients(Guid clinicSectionId);
        IEnumerable<PatientViewModel> GetAllReceptionClinicSectionPatients();
    }
}
