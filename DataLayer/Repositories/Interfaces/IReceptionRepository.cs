using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceptionRepository : IRepository<Reception>
    {
        IEnumerable<Reception> GetAllReception();
        IEnumerable<Reception> GetAllReceptionsByClinicSection(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null);
        IEnumerable<Reception> GetAllAllHospitalPatientReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<Reception, bool>> predicate = null);
        IEnumerable<Reception> GetAllAllHospitalPatientReportStimul(Guid clinicSectionId, Expression<Func<Reception, bool>> predicate = null);
        List<Reception> GetReceptionsByClinicSectionForCash(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null);
        Reception GetReception(Guid receptionId);
        IEnumerable<PatientImage> RemoveReception(Guid receptionid);
        Guid UpdateReception(Reception reception);
        IEnumerable<User> GetReceptionPatient();
        Reception GetReceptionWithServices(Guid receptionId);
        void UpdateReceptionCleareance(Reception rt);
        void UpdateReceptionChiefComplaint(Reception rt);
        bool GetReceptionDischargeStatus(Guid receptionid);
        IEnumerable<ReceptionForCashReport> GetAllReceptionsByClinicSectionForCashReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string statusId);
        List<Reception> GetAllReceptionsForSelectRoomBed(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid receptionId, Expression<Func<Reception, bool>> predicate = null);
        Reception GetReceptionWithServiceByReserveDetailId(Guid reserveDetailId);
        IEnumerable<Reception> GetAllReceptionForOneDayBasedOnDoctorIdJustStatusAndVisitNum(Guid doctorId, DateTime date);
        Reception GetTodayReceptionThatMustVisitingByDoctorId(Guid doctorId, DateTime today, bool lastVisit, int visitNum = 0);
        Reception GetMedicineReceptionForServer(Guid receptionId);
        Reception GetAnalysisReceptionForServer(Guid receptionId);
        Reception GetReceptionWithPatient(Guid receptionId);
        string GetServerVisitNum(Guid receptionId);
        IEnumerable<DateTime> GetReceptionCount(Guid clinicSectionId);
    }
}
