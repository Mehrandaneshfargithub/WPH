using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceptionServiceReceivedRepository : Repository<ReceptionServiceReceived>, IReceptionServiceReceivedRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionServiceReceivedRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<ReceptionServiceReceived> GetReceptionServiceReceivedsByReceptionId(Guid receptionId)
        {
            return Context.ReceptionServiceReceiveds.AsNoTracking()
                .Where(p => p.ReceptionServiceId == receptionId);
        }

        public ReceptionServiceReceived GetReceptionServiceReceivedWithReception(Guid id)
        {
            return Context.ReceptionServiceReceiveds.AsNoTracking()
                .SingleOrDefault(p => p.Guid == id);
        }

        public IEnumerable<ReceptionServiceReceived> GetAllReceptionServiceRecievedForInstallment(Guid receptionId)
        {
            return Context.ReceptionServiceReceiveds.Include(a=>a.ReceptionService).AsNoTracking()
                .Where(p => p.ReceptionService.ReceptionId == receptionId && !p.AmountStatus.Value)
                .Select(a=>new ReceptionServiceReceived
                {
                    Amount = a.Amount,
                    CreatedDate = a.CreatedDate.Value.Date,
                    InstallmentId = a.InstallmentId
                });
        }

        public IEnumerable<ReceptionServiceReceived> GetAllClinicInCome(Guid userId)
        {
            return Context.ReceptionServiceReceiveds.Include(a => a.ReceptionService).ThenInclude(a=>a.Reception).AsNoTracking()
                .Where(p => p.AmountStatus == false && Context.ClinicSectionUsers.Where(a=>a.UserId == userId ).Select(a=>a.ClinicSectionId).Contains(p.ReceptionService.Reception.ClinicSectionId.Value))
                .Select(a => new ReceptionServiceReceived
                {
                    Amount = a.Amount,
                    CreatedDate = a.CreatedDate.Value.Date
                });
        }
    }
}
