
using System;
using System.Collections.Generic;
using WPH.Models.Receive;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReceiveMvcMockingService
    {
        string RemoveReceive(Guid Receiveid, Guid userId, string pass);
        string AddNewInvoiceReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string UpdateInvoiceReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string AddNewPartialReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string UpdatePartialReceive(ReceiveViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string AddNewReceive(ReceiveViewModel Receive);
        string UpdateReceive(ReceiveViewModel Receive);
        ReceiveViewModel GetReceive(Guid ReceiveId);
        IEnumerable<ReceiveViewModel> GetPartialReceiveHistory(IEnumerable<string> receiveIds);
        void GetModalsViewBags(dynamic viewBag);
        decimal GetSaleInvoiceReceives(Guid saleInvoiceId, int currencyId);
        IEnumerable<ReceiveAmountViewModel> GetAllReceiveAmount(Guid saleInvoiceId, int currencyId);
        string RemoveReceiveAmount(Guid id);
    }
}
