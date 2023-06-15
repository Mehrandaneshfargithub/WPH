using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface ITransferRepository : IRepository<Transfer>
    {
        IEnumerable<Transfer> GetAllTransfer(DateTime dateFrom, DateTime dateTo, Expression<Func<Transfer, bool>> predicate = null);
        Transfer GetWithType(Guid transferId);
        IEnumerable<string> GetReceiversName(List<Guid> clinicSections);
        IEnumerable<Transfer> GetAllProductRecive(Guid clinicSectionId);
        void RemoveTransfer(Transfer transfer);
    }
}
