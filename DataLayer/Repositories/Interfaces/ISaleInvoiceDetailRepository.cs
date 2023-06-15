using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISaleInvoiceDetailRepository : IRepository<SaleInvoiceDetail>
    {
        IEnumerable<SaleInvoiceDetail> GetAllSaleInvoiceDetailByMasterId(Guid clinicSectionId);
        IEnumerable<SaleInvoiceDetail> GetByMultipleIds(List<Guid> details);
        IEnumerable<SaleInvoiceDetail> GetDetailsForReturn(Guid productId, Guid masterId, Guid clinicSectionId, bool like);
        SaleInvoiceDetail GetWithPurchaseAndTransfer(Guid detailId);
        IEnumerable<PieChartModel> GetMostSaledProducts(Guid clinicSectionId);
        IEnumerable<SaleInvoiceDetail> GetAllDetail(IEnumerable<Guid> saleInvoiceDetailIds);
        void UpdatePurchaseAndTransferStock(List<PurchaseOrTransferProductDetail> allupdates);
    }
}
