using DataLayer.EntityModels;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using WPH.Models.PurchaseInvoice;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPurchaseInvoiceMvcMockingService
    {
        string RemovePurchaseInvoice(Guid PurchaseInvoiceid, Guid userId, string pass);
        string AddNewPurchaseInvoice(PurchaseInvoiceViewModel viewModel);
        string UpdatePurchaseInvoice(PurchaseInvoiceViewModel viewModel);
        IEnumerable<PurchaseInvoiceViewModel> GetAllPurchaseInvoices(Guid clinicSectionId, PurchaseInvoiceFilterViewModel filterViewModel, IStringLocalizer<SharedResource> _localizer);
        PurchaseInvoiceViewModel GetPurchaseInvoice(Guid PurchaseInvoiceId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<PurchaseInvoiceTotalPriceViewModel> GetAllTotalPrice(Guid purchaseInvoiceId);
        string UpdateTotalPrice(PurchaseInvoice invoice);
        IEnumerable<PurchaseInvoiceViewModel> GetPayPurchaseInvoice(Guid? payId);
        IEnumerable<PurchaseInvoiceViewModel> GetNotPayPurchaseInvoice(Guid? supplierId);
        IEnumerable<PurchaseInvoiceViewModel> GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId);
        IEnumerable<PurchaseInvoiceViewModel> GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId);
        PrintPurchaseReportViewModel GetPurchaseForReport(Guid purchaseInvoiceId, IStringLocalizer<SharedResource> _localizer);
        bool CheckPurchaseInvoicePaid(Guid purchaseInvoiceId);
    }
}
