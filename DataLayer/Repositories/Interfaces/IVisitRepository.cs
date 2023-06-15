using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IVisitRepository : IRepository<Reception>
    {
        IEnumerable<Reception> GetAllVisitByClinicSection(Guid clinicSection);

        Reception GetVisitDetailBasedOnId(Guid visitId);
        IEnumerable<Reception> GetAllVisitForOneDayBasedOnDoctorId(Guid doctorId, DateTime? Date);
        IEnumerable<Reception> GetAllPatientVisitByClinicSection(Guid patientId);
        Reception GetVisitBasedOnReserveDetailId(Guid ReserveDetailId);
        IEnumerable<Reception> GetAllVisitForSpecificDateBasedOnUserAccess(List<Guid> doctors, DateTime dateFrom, DateTime dateTo);
        IEnumerable<Reception> GetAllVisitForSpecificDateByDoctorId(Guid doctorId, DateTime dateFrom, DateTime dateTo);
        int GetAllTodayVisitsCountBasedOnClinicSection(Guid clinicSectionId, DateTime today);
        Task UpdateReceptionNums(Guid doctorId);
        void AddNewVisit(Reception vis, List<PatientVariablesValue> addValues, List<PatientVariablesValue> updatedValues);
        void UpdateVisit(Reception vis, List<PatientVariablesValue> add, List<PatientVariablesValue> update);
        void UpdateVisitStatus(Reception vis);
        void RemoveVisit(Guid id);
        Reception GetVisitById(Guid visitId);
        int GetLastClinicSectionVisitNum(Guid clinicSectonId, DateTime date);
        Guid? CheckVisitExistByReserveDetailId(Guid reserveDetailId);
        Reception GetVisitWithReserveDetailId(Guid visitId);
        Reception GetVisitForReportById(Guid visitId);
    }
}
