using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.ReserveDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReserveDetailMvcMockingService
    {
        List<EventViewModel> GetAllReservesBetweenTwoDate(Guid originalClinicSectionId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, DateTime calDate, DateTime today, Guid doctorId);
        ReserveDetailViewModel GetReserveAllDetail(Guid resAllD);
        void AddNewReserveDetail(ReserveDetailViewModel resDetail, bool newPatient, Guid clinicSectionId);
        string RemoveReserveDetail(Guid reserveDetailId);
        string UpdateReserveDetail(ReserveDetailViewModel resDetail, bool newPatient, Guid clinicSectionId);
        void UpdateReserveDetailStatus(ReserveDetailViewModel resAllD);
        DateTime? GetLastPatientVisitDate(Guid patientId, bool recieved);
        void UpdateReserveDetailTime(Guid Reserveid, string start, string end);
        PatientViewModel GetPatientIdAndNameFromReserveDetailId(Guid reserveDetailId);
        Guid GetReserveDetailDoctorId(Guid reserveDetailId);
    }
}
