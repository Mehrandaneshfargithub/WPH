using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPayRepository : IRepository<Pay>
    {
        string GetLatestPayInvoiceNum(Guid clinicSectionId);
        Pay GetWithSupplier(Guid payId);
        Pay GetWithPayAmount(Guid payId);
        Pay GetPayWithPurchaseInvocie(Guid payId);
        string CheckPayStatus(Guid payId);
        IEnumerable<Pay> GetPartialPayHistory(IEnumerable<string> ids);
    }
}
