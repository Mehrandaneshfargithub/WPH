using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.Reception;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPatientReceptionMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        string GetLatestReceptionInvoiceNum(Guid clinicSectionId);
        string NextInvoiceNum(string str);
        //ReceptionViewModel GetPatientReceptionBasedOnPatientReceptionIdWithRecievesAndAnalysis(Guid id);
        ReceptionViewModel GetPatientReceptionByIdForReport(Guid patienReceptionId);
        string GetTodaysFirstReceptionInvoiceNum(Guid clinicSectionId, DateTime today);
        //ReceptionViewModel GetPatientReceptionForAnalysisResultReport(Guid patientReceptionId);
        ReceptionViewModel GetPatientReceptionByIdForAnalysisResult(Guid id);
        //IEnumerable<ReceptionViewModel> GetAllPatientReception(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo);
        ///ReceptionViewModel GetPatientReceptionByInvoiceNum(string invoiceNum, Guid clinicSectionId);
        Guid AddNewPatientReception(ReceptionViewModel newPatientReception, bool newPatient, bool newDoctor);
        string BarcodeGenerator(Guid clinicSectionId);
        bool CheckRepeatedBarcode(Guid clinicSectionId, string barcode);
        IEnumerable<ReceptionViewModel> GetAllPatientReceptionInvoiceNums(Guid clinicSectionId);
        IEnumerable<ReceptionViewModel> GetAllPatientReceptionPatients(Guid clinicSectionId);
        Guid UpdatePatientReception(ReceptionViewModel PatientReception, bool newPatient, bool newDoctor);
        //OperationStatus RemovePatientReception(Guid PatientReceptionId);
        OperationStatus RemovePatientReceptionWithReceives(Guid PatientReceptionId, string rootPath);
        //IEnumerable<ReceptionViewModel> GetAllPatientReceptionByUserId(Guid userId, int periodId, DateTime DateFrom, DateTime DateTo);
        Guid AddOrUpdate(ReceptionViewModel patientReception);
        ReceptionViewModel GetPatientReceptionByIdWithDoctor(Guid receptionId);
        ReceptionViewModel GetPatientReceptionById(Guid receptionId);
    }
}
