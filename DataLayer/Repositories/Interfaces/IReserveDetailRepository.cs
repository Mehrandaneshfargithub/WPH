using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReserveDetailRepository : IRepository<ReserveDetail>
    {
        ReserveDetail GetReserveDetail(Guid resAllD);
        IEnumerable<ReserveDetail> GetAllReservesBetweenTwoDateBasedOnUserAccess(List<Guid> doctors, DateTime fromDate, DateTime toDate);
        IEnumerable<FN_GetAllEventsForCalendar_Result> GetAllReservesBetweenTwoDateForCalendar(Guid originalClinicSectionId, Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid doctorId);
        DateTime? GetLastPatientVisitDate(Guid patientId, bool recieved);
        void UpdateReserveDetailTime(Guid reserveid, string start, string end);
        void AddNewReserveDetail(ReserveDetail res, bool newPatient);
        void UpdateReserveDetail(ReserveDetail res, bool newPatient);
        string RemoveReserveDetail(Guid reserveDetailId);
        void UpdateReserveDetailStatus(ReserveDetail res);
        IEnumerable<ReserveDetail> GetAllReservesBetweenTwoDateByDocotrId(Guid doctorId, DateTime fromDate, DateTime toDate);
        ReserveDetail GetReserveDetailDoctorId(Guid reserveDetailId);
        ReserveDetail GetWithReception(Guid reserveDetailId);
        ReserveDetail GetLastReserveDetail(Guid reserveDetailId, Guid? patientId);
        ReserveDetail GetNoTracking(Guid reserveDetailId);
    }
}
