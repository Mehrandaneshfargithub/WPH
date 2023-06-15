using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnPurchaseInvoiceDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IReturnPurchaseInvoiceDetailMvcMockingService
    {
        string RemoveReturnPurchaseInvoiceDetail(Guid ReturnPurchaseInvoiceDetailid);
        string AddNewReturnPurchaseInvoiceDetail(ReturnPurchaseInvoiceDetailViewModel ReturnPurchaseInvoiceDetail, Guid clinicSectionId);
        string AddNewReturnPurchaseInvoiceDetailList(IEnumerable<ReturnPurchaseInvoiceDetailViewModel> viewModels, Guid clinicSectionId, Guid userId);
        string UpdateReturnPurchaseInvoiceDetail(ReturnPurchaseInvoiceDetailViewModel ReturnPurchaseInvoiceDetail, Guid clinicSectionId);
        IEnumerable<ReturnPurchaseInvoiceDetailViewModel> GetAllReturnPurchaseInvoiceDetails(Guid clinicSectionId);
        IEnumerable<SubReturnPurchaseInvoiceDetailViewModel> GetAllReturnPurchaseInvoiceDetailChildren(string children);
        ReturnPurchaseInvoiceDetailViewModel GetReturnPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
