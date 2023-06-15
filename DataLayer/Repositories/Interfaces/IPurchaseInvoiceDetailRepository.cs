using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPurchaseInvoiceDetailRepository : IRepository<PurchaseInvoiceDetail>
    {
        IEnumerable<PurchaseInvoiceDetail> GetAllPurchaseInvoiceDetailByMasterId(Guid clinicSectionId);
        decimal GetSourceRemCount(Guid purchaseInvoiceDetailId);
        IEnumerable<PurchaseInvoiceDetail> GetAllTotalPrice(Guid purchaseInvoiceId);
        PurchaseInvoiceDetail GetPurchaseInvoiceDetailForEdit(Guid purchaseInvoiceDetailId);
        IEnumerable<PurchaseInvoiceDetail> GetPurchaseHistoryByProductId(Guid clinicSectionId, Guid productId);
        IEnumerable<ReturnPurchaseDetailModel> GetDetailsForReturn(Guid productId, Guid masterId, Guid clinicSectionId, bool like);
        IEnumerable<PurchaseInvoiceDetail> GetByMultipleIds(List<Guid> details);
        PurchaseOrTransferProductDetail GetProductDetails(Guid productId, bool latestPrice, string SaleType, int? SellCurrencyId);
        IEnumerable<PurchaseOrTransferProductDetail> GetAllProductExpireList(Guid id);
        PurchaseOrTransferProductDetail GetProductDetailsFromExpireList(Guid invoiceId, string invoiceType, string SaleType, int? SellCurrencyId, bool latestPrice, Guid productId);
        void IncreaseUpdateWithLocal(PurchaseInvoiceDetail detail, decimal remainingNum);
        IEnumerable<PieChartModel> GetProductStocks(Guid clinicSectionId);
        PurchaseInvoiceDetail GetForNewSalePrice(Guid purchaseInvoiceDetailId);
        PurchaseInvoiceDetail GetForSalePrice(Guid purchaseInvoiceDetailId);
        PurchaseInvoiceDetail GetParentCurrency(Guid purchaseInvoiceDetailId);
        IEnumerable<PurchaseInvoiceDetailPriceModel> GetProductLastPricesByProducId(Guid productId, Guid transferId);
        IEnumerable<PurchaseInvoiceDetail> GetWithPricesByMultipleIds(List<Guid> details);
        PurchaseInvoiceDetail GetWithPricesById(Guid purchaseInvoiceDetailId);
        IEnumerable<ExpireListModel> GetExpiredList(Guid clinicSectionId, string type);
        bool CheckDetailInUse(Guid detailId);
    }
}
