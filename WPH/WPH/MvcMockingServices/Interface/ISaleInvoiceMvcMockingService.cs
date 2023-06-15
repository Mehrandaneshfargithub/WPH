using DataLayer.EntityModels;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.SaleInvoice;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISaleInvoiceMvcMockingService
    {
        string RemoveSaleInvoice(Guid SaleInvoiceid, Guid userId, string pass);
        string AddNewSaleInvoice(SaleInvoiceViewModel SaleInvoice);
        string UpdateSaleInvoice(SaleInvoiceViewModel SaleInvoice);
        IEnumerable<SaleInvoiceViewModel> GetAllSaleInvoices(Guid clinicSectionId, SaleInvoiceFilterViewModel filter, IStringLocalizer<SharedResource> _localizer);
        SaleInvoiceViewModel GetSaleInvoice(Guid SaleInvoiceId);
        void GetModalsViewBags(dynamic viewBag);
        string UpdateTotalPrice(IEnumerable<SaleInvoiceDetail> saleInvoiceId, SaleInvoiceDiscount saleInvoiceDiscount, string type, string currencyname);
        IEnumerable<SaleInvoiceViewModel> GetReceiveSaleInvoice(Guid? ReceiveId);
        IEnumerable<SaleInvoiceViewModel> GetNotReceiveSaleInvoice(Guid? customerId);
        string GetTotalPrice(Guid saleInvoiceId);
        IEnumerable<PieChartViewModel> GetAllIncomes(Guid clinicSectionId);
        bool CheckSaleInvoiceRecieve(Guid id);
        SaleInvoiceViewModel GetSaleInvoiceByReceiveId(Guid receiveId);
        List<IncomeViewModel> GetAllStoreInCome(Guid clinicSectionId, string type);
    }
}
