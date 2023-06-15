using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReturnSaleInvoiceDetailRepository : IRepository<ReturnSaleInvoiceDetail>
    {
        bool CheckDetailsExistByMasterId(Guid? masterId);
        bool CheckDetailsExistBySaleInvoiceDetailIds(List<Guid?> saleInvoiceDetailIds);
        IEnumerable<ReturnSaleInvoiceDetail> GetAllReturnSaleInvoiceDetailByMasterId(Guid returnSaleInvoiceId);
        IEnumerable<ReturnSaleInvoiceDetail> GetAllReturnSaleInvoiceDetailByIds(List<Guid> ids);
        //decimal GetSourceRemCount(Guid saleInvoiceDetailId);
        IEnumerable<ReturnSaleInvoiceDetail> GetAllTotalPrice(Guid returnSaleInvoiceId);
        ReturnSaleInvoiceDetail GetReturnSaleInvoiceDetailForEdit(Guid returnSaleInvoiceDetailId);
        ReturnSaleInvoiceDetail GetWithSaleInvoice(Guid returnSaleInvoiceDetailId);
        ReturnSaleInvoiceDetail GetWithPurchaseAndTransfer(Guid returnSaleInvoiceId);
        IEnumerable<PieChartModel> GetMostReturnedProducts(Guid clinicSectionId);
    }
}
