using System;
using System.Collections.Generic;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.Models.ReturnPurchaseInvoiceDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface IPurchaseInvoiceDetailMvcMockingService
    {
        string RemovePurchaseInvoiceDetail(Guid PurchaseInvoiceDetailid);
        string AddNewPurchaseInvoiceDetail(PurchaseInvoiceDetailViewModel PurchaseInvoiceDetail, Guid clinicSectionId);
        string UpdatePurchaseInvoiceDetail(PurchaseInvoiceDetailViewModel PurchaseInvoiceDetail, Guid clinicSectionId);
        IEnumerable<PurchaseInvoiceDetailViewModel> GetAllPurchaseInvoiceDetails(Guid clinicSectionId);
        PurchaseInvoiceDetailViewModel GetPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId); 
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<PurchaseInvoiceDetailHistoryViewModel> GetPurchaseHistory(Guid clinicSectionId, Guid productId);
        IEnumerable<ReturnPurchaseInvoiceDetailSelectViewModel> GetDetailsForReturn(Guid masterId, Guid productId, Guid clinicSectionId, bool like);
        ParentDetailSalePriceViewModel GetForNewSalePrice(Guid purchaseInvoiceDetailId);
        Guid? GetPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailId, int currencyId, string priceType, string saleType);
    }
}
