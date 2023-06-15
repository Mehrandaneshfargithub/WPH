using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.ReturnSaleInvoiceDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnSaleInvoiceDetailMvcMockingService
    {
        string RemoveReturnSaleInvoiceDetail(Guid ReturnSaleInvoiceDetailid);
        string AddNewReturnSaleInvoiceDetail(ReturnSaleInvoiceDetailViewModel ReturnSaleInvoiceDetail, Guid clinicSectionId);
        string AddNewReturnSaleInvoiceDetailList(IEnumerable<ReturnSaleInvoiceDetailViewModel> viewModels, Guid clinicSectionId, Guid userId);
        string UpdateReturnSaleInvoiceDetail(ReturnSaleInvoiceDetailViewModel ReturnSaleInvoiceDetail, Guid clinicSectionId);
        IEnumerable<ReturnSaleInvoiceDetailViewModel> GetAllReturnSaleInvoiceDetails(Guid clinicSectionId);
        IEnumerable<SubReturnSaleInvoiceDetailViewModel> GetAllReturnSaleInvoiceDetailChildren(string children);
        ReturnSaleInvoiceDetailViewModel GetReturnSaleInvoiceDetailForEdit(Guid saleInvoiceDetailId);
        void GetModalsViewBags(dynamic viewBag);
        PieChartViewModel GetMostReturnedProducts(Guid clinicSectionId);
    }
}
