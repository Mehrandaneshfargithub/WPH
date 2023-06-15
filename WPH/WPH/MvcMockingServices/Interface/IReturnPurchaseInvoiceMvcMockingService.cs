
using DataLayer.EntityModels;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoice;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnPurchaseInvoiceMvcMockingService
    {
        string RemoveReturnPurchaseInvoice(Guid returnPurchaseInvoiceid, Guid userId, string pass);
        string AddNewReturnPurchaseInvoice(ReturnPurchaseInvoiceViewModel viewModel);
        string UpdateReturnPurchaseInvoice(ReturnPurchaseInvoiceViewModel viewModel);
        IEnumerable<ReturnPurchaseInvoiceViewModel> GetAllReturnPurchaseInvoices(Guid clinicSectionId, ReturnPurchaseInvoiceFilterViewModel filterViewModel);
        ReturnPurchaseInvoiceViewModel GetReturnPurchaseInvoice(Guid returnPurchaseInvoiceId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<ReturnPurchaseInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid returnPurchaseInvoiceId);
        string UpdateTotalPrice(ReturnPurchaseInvoice invoice);
        IEnumerable<ReturnPurchaseInvoiceViewModel> GetPayReturnPurchaseInvoice(Guid? payId);
        IEnumerable<ReturnPurchaseInvoiceViewModel> GetNotPayReturnPurchaseInvoice(Guid? supplierId);
        IEnumerable<ReturnPurchaseInvoiceViewModel> GetPartialPayReturnPurchaseInvoice(Guid? payId);
        IEnumerable<ReturnPurchaseInvoiceViewModel> GetNotPartialPayReturnPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId);
        bool CheckReturnPurchaseInvoicePaid(Guid returnPurchaseInvoiceId);
    }
}
