using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceptionServiceReceivedRepository : IRepository<ReceptionServiceReceived>
    {
        IEnumerable<ReceptionServiceReceived> GetReceptionServiceReceivedsByReceptionId(Guid receptionId);
        ReceptionServiceReceived GetReceptionServiceReceivedWithReception(Guid id);
        IEnumerable<ReceptionServiceReceived> GetAllReceptionServiceRecievedForInstallment(Guid receptionId);
        IEnumerable<ReceptionServiceReceived> GetAllClinicInCome(Guid userId);
    }
}
