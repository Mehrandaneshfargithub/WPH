using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.CustomDataModels.PatientReception;
using WPH.Models.CustomDataModels.ReserveDetail;
using WPH.Models.HospitalPatients;
using WPH.Models.Reception;
using WPH.Models.ReceptionDoctor;
using WPH.Models.ReceptionTemperature;

namespace WPH.MvcMockingServices.Interface
{

    public interface IReceptionMvcMockingService
    {
        OperationStatus RemoveReception(Guid Receptionid, string rootPath);
        string AddNewReception(ReceptionViewModel Reception);

        //IEnumerable<ReceptionViewModel> GetAllReceptions();
        ReceptionViewModel GetReception(Guid ReceptionId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<PatientReceptionViewModel> GetAllReceptionsByClinicSection(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate, Guid receptionId, int status);
        ShowHospitalPatientReportResultViewModel AllHospitalPatientReport(HospitalPatientReportViewModel reportViewModel);
        IEnumerable<PatientReceptionViewModel> GetReceptionsByClinicSectionForCash(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate, Guid receptionId, int paymentStatus);
        IEnumerable<ReceptionPatientNameViewModel> GetReceptionPatientName();
        IEnumerable<ReceptionDoctorViewModel> GetReceptionDoctor(Guid receptionId);
        IEnumerable<ReceptionTemperatureViewModel> GetAllReceptionTemperature(Guid receptionId);
        PieChartViewModel GetReceptionCount(Guid clinicSectionId);
        void AddNewReceptionTemperature(ReceptionTemperatureViewModel reception);
        void UpdateReceptionCleareance(ReceptionViewModel reception);
        OperationStatus RemoveReceptionTemperature(Guid id);
        void UpdateReceptionChiefComplaint(ReceptionViewModel reception);
        string DischargePatient(Guid id, Guid userId, bool confirm);
        ReceptionViewModel GetReceptionOnly(Guid receptionId);
        IEnumerable<ReceptionForCashReportViewModel> GetAllReceptionsByClinicSectionForCashReport(Guid clinicSectionId, int periodId, DateTime dateFrom, DateTime dateTo, string status);
        IEnumerable<HospitalPatientReportResultViewModel> GetAllReceptionsForHospitalPatients(Guid id, int periodId, DateTime fromDate, DateTime toDate, int status);
        IEnumerable<PatientReceptionViewModel> GetAllReceptionsForSelectRoomBed(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo, Guid receptionId, int status);
        Task<string> AddReceptionForReserve(ReserveDetailViewModel viewModel);
        Task<decimal?> GetReceptionRemByReserveDetailId(Guid reserveDetailId, Guid clinicSectionId);
        string GetServerVisitNum(Guid receptionId);
    }
}
