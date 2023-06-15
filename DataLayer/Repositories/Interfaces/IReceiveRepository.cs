using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReceiveRepository : IRepository<Receive>
    {
        string GetLatestReceiveInvoiceNum(Guid clinicSectionId);
        Receive GetWithCustomer(Guid receiveId);
        Receive GetWithReceiveAmount(Guid receiveId);
        Receive GetReceiveWithSaleInvocie(Guid receiveId);
        string CheckReceiveStatus(Guid receiveId);
        IEnumerable<Receive> GetPartialReceiveHistory(IEnumerable<string> ids);
        decimal GetSaleInvoiceReceives(Guid saleInvoiceId, int currencyId);
    }
}
