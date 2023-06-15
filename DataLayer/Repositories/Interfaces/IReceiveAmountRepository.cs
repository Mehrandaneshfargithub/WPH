using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceiveAmountRepository : IRepository<ReceiveAmount>
    {
        IEnumerable<ReceiveAmount> GetAllReceiveAmount(Guid saleInvoiceId, int currencyId);
    }
}
