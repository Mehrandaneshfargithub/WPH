using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.HospitalPatients;
using WPH.Models.ReceptionClinicSection;

namespace WPH.MvcMockingServices.Interface
{
    
    public interface IReceptionClinicSectionMvcMockingService
    {
        OperationStatus RemoveReceptionClinicSection(Guid ReceptionClinicSectionid);
        Guid AddNewReceptionClinicSection(ReceptionClinicSectionViewModel ReceptionClinicSection);
        Guid UpdateReceptionClinicSection(ReceptionClinicSectionViewModel Hosp);
        //ReceptionClinicSectionViewModel GetReceptionClinicSection(Guid ReceptionClinicSectionId);
        IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSections();
        ReceptionClinicSectionViewModel GetReceptionClinicSection(Guid ReceptionClinicSectionId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSectionByReceptionId(Guid ReceptionId);
        IEnumerable<ReceptionClinicSectionViewModel> GetAllReceptionClinicSectionByClinicSectionId(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate, Guid receptionId, int status);
        ReceptionClinicSectionViewModel GetReceptionClinicSectionByDestinationReceptionId(Guid DestinationReceptionId);
        ShowPatientToAnotherSectionReportResultViewModel GetPatientToAnotherSectionReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate);
        
    }
}
