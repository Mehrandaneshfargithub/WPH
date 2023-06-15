using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceptionServiceRepository : IRepository<ReceptionService>
    {
        IEnumerable<ReceptionService> GetReceptionServicesByReceptionId(Guid receptionId);
        ReceptionService GetReceptionServiceWithRecives(Guid id);
        IEnumerable<ReceptionService> GetAllReceptionServices(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo);
        ReceptionService GetReceptionExceptOperationService(Guid receptionId);
        IEnumerable<ReceptionService> GetReceptionSpecificServicesByReceptionId(Guid receptionId, string serviceType);
        IEnumerable<ReceptionService> GetUnpaidReceptionServicesByReceptionId(Guid receptionId);
        ReceptionService GetReceptionOperation(Guid receptionId);
        IEnumerable<ReceptionService> GetAllReceptionProducts(Guid receptionId);
        IEnumerable<ReceptionService> GetAllReceptionDMSProducts(Guid receptionId);
        ReceptionService GetReceptionServiceWithReception(Guid id);
        ReceptionService GetReceptionOperationAndDoctor(Guid receptionId);
        ReceptionService GetFirstOrDefault(Expression<Func<ReceptionService, bool>> predicate = null);
        decimal? GetReceptionServiceRem(Guid receptionServiceId);
        ReceptionService GetReceptionServiceWithPatient(Guid receptionServiceId);
        bool HasDoctorVisit(Guid receptionId);
        decimal? GetReceptionRemByReceptionId(Guid receptionId);
        IEnumerable<ReceptionService> GetVisitPriceByReserveDetailId(Guid reserveDetailId);
        IEnumerable<PieChartModel> GetMostOperations(Guid userId);
    }
}
