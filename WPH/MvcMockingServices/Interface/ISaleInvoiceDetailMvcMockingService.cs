using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.ReturnSaleInvoiceDetail;
using WPH.Models.SaleInvoiceDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISaleInvoiceDetailMvcMockingService
    {
        string RemoveSaleInvoiceDetail(IEnumerable<Guid> SaleInvoiceDetailid);
        string AddNewSaleInvoiceDetail(SaleInvoiceDetailViewModel SaleInvoiceDetail);
        string UpdateSaleInvoiceDetail(SaleInvoiceDetailViewModel SaleInvoiceDetail);
        IEnumerable<SaleInvoiceDetailViewModel> GetAllSaleInvoiceDetails(Guid clinicSectionId);
        SaleInvoiceDetailViewModel GetSaleInvoiceDetail(Guid SaleInvoiceDetailId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<ReturnSaleInvoiceDetailSelectViewModel> GetDetailsForReturn(Guid masterId, Guid productId, Guid clinicSectionId, bool like);
        IEnumerable<SaleInvoiceDetailViewModel> GetAllDetail(IEnumerable<Guid> saleInvoiceDetailIds);
    }
}
