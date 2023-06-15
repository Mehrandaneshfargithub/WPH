using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Pay;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPayMvcMockingService
    {
        string RemovePay(Guid Payid, Guid userId, string pass);
        string AddNewInvoicePay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string UpdateInvoicePay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string AddNewPartialPay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string UpdatePartialPay(PayViewModel viewModel, IEnumerable<Guid> invoiceIds, IEnumerable<Guid> returnIds);
        string AddNewPay(PayViewModel Pay);
        string UpdatePay(PayViewModel Pay);
        PayViewModel GetPay(Guid PayId);
        IEnumerable<PayViewModel> GetPartialPayHistory(IEnumerable<string> payIds);
        void GetModalsViewBags(dynamic viewBag);
    }
}
