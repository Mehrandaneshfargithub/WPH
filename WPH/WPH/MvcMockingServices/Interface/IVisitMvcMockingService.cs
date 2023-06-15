using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Visit;
using WPH.Models.CustomDataModels.Visit_Patient_Disease;
using WPH.Models.CustomDataModels.Visit_Symptom;
using WPH.Models.Visit;

namespace WPH.MvcMockingServices.Interface
{
    public interface IVisitMvcMockingService
    {
        Task<Guid> AddNewVisit(VisitViewModel viewModel);
        VisitViewModel GetVisitBasedOnReserveDetailId(Guid reserveDetailId);
        VisitViewModel GetVisitBasedOnReceptionId(Guid receptionId);
        IEnumerable<VisitViewModel> GetAllVisitForOneDayBasedOnDoctorId(Guid doctorId, DateTime Date);
        IEnumerable<VisitViewModel> GetAllPatientVisitByClinicSection(Guid PatientId);
        VisitViewModel GetVisitById(Guid visitId);
        VisitForReportViewModel GetVisitForReportById(Guid visitId);
        IEnumerable<VisitViewModel> GetAllVisitForOneDayBasedOnDoctorIdJustStatusAndVisitNum(Guid doctorId, DateTime date);
        VisitViewModel GetTodayTheVisitThatMustVisitingByDoctorId(Guid doctorId, DateTime today, int visitNum = 0);
        List<VisitViewModel> GetAllVisitForSpecificDateBasedOnUserAccess(List<Guid> doctors, int periodId, DateTime dateFrom, DateTime dateTo);
        List<VisitViewModel> GetAllVisitForSpecificDateByDoctorId(Guid doctorId, int periodId, DateTime dateFrom, DateTime dateTo);
        IEnumerable<Guid> GetAllVisitDiseaseWithJustDiseaseID(Guid visitId);
        List<Visit_SymptomViewModel> GetAllVisitSymptom(Guid visitId);
        IEnumerable<Guid> GetAllVisitSymptomWithJustSymptomID(Guid visitId);
        void UpdateVisit(VisitViewModel visit);
        List<Visit_Patient_DiseaseViewModel> GetAllVisitDisease(Guid visitId);
        void RemoveVisitDiseasePatientRange(List<Visit_Patient_DiseaseViewModel> visitDisease);
        void AddVisitDiseasePatientRange(List<Visit_Patient_DiseaseViewModel> visitDisease);
        void RemoveDiseaseFromVisit(string diseaseId, Guid visitId);
        void AddSymptomToVisit(Guid symptomId, Guid visitId);
        void RemoveSymptomFromVisit(Guid symptomId, Guid visitId);
        Guid AddVisitDiseasePatient(Visit_Patient_DiseaseViewModel vpd);
        OperationStatus RemoveVisit(Guid VisitId);
        string GetAllPrescriptionForHistory(Guid VisitId);
        string GetAllPrescriptionTestForHistory(Guid VisitId);
        void UpdateReceptionNums(Guid doctorId);
        Task UpdateVisitStatus(VisitViewModel visit);
        string SendVisitToServer(Guid visitId, string baseurl);
        string SendAnalysisToServer(Guid visitId, string baseurl);
        GetVisitFromServerViewModel GetVisitFromServer(Guid visitId, string baseurl);
        GetVisitFromServerViewModel GetAnalysisFromServerByVisitId(Guid visitId, string baseurl);
        Guid? CheckVisitExistByReserveDetailId(Guid reserveDetailId);
        string AddPayVisit(PayVisitViewModel viewModel);
        string PayVisit(Guid receptionServiceId, Guid userId);
        string PayAllVisit(Guid receptionId, Guid userId);
        GetVisitFromServerViewModel GetAnalysisFromServerByNameMobile(string baseurl, long? analysisServerVisitNum, string patientMobileName);
    }
}
