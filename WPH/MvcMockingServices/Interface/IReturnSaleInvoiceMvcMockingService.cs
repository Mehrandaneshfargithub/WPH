using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using WPH.Models.Chart;
using WPH.Models.ReturnSaleInvoice;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnSaleInvoiceMvcMockingService
    {
        string RemoveReturnSaleInvoice(Guid returnSaleInvoiceid, Guid userId, string pass);
        string AddNewReturnSaleInvoice(ReturnSaleInvoiceViewModel viewModel);
        string UpdateReturnSaleInvoice(ReturnSaleInvoiceViewModel viewModel);
        IEnumerable<ReturnSaleInvoiceViewModel> GetAllReturnSaleInvoices(Guid clinicSectionId, ReturnSaleInvoiceFilterViewModel filterViewModel);
        ReturnSaleInvoiceViewModel GetReturnSaleInvoice(Guid returnSaleInvoiceId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<ReturnSaleInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid returnSaleInvoiceId);
        string UpdateTotalPrice(ReturnSaleInvoice invoice);
        IEnumerable<ReturnSaleInvoiceViewModel> GetReceiveReturnSaleInvoice(Guid? receiveId);
        IEnumerable<ReturnSaleInvoiceViewModel> GetNotReceiveReturnSaleInvoice(Guid? customerId);
        bool CheckReturnSaleInvoiceReceived(Guid returnSaleInvoiceId);
        //IEnumerable<ReturnSaleInvoiceViewModel> GetPartialReceiveReturnSaleInvoice(Guid? receiveId);
        //IEnumerable<ReturnSaleInvoiceViewModel> GetNotPartialReceiveReturnSaleInvoice(Guid? customerId, int? currencyId, Guid? receiveId);
    }
}
