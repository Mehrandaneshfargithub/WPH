using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReturnPurchaseInvoiceDetailRepository : IRepository<ReturnPurchaseInvoiceDetail>
    {
        bool CheckDetailsExistByMasterId(Guid? masterId);
        bool CheckDetailsExistByPurchaseInvoiceDetailIds(List<Guid?> purchaseInvoiceDetailIds);
        IEnumerable<ReturnPurchaseInvoiceDetail> GetAllReturnPurchaseInvoiceDetailByMasterId(Guid returnPurchaseInvoiceId);
        IEnumerable<ReturnPurchaseInvoiceDetail> GetAllReturnPurchaseInvoiceDetailByIds(List<Guid> ids);
        //decimal GetSourceRemCount(Guid purchaseInvoiceDetailId);
        IEnumerable<ReturnPurchaseInvoiceDetail> GetAllTotalPrice(Guid returnPurchaseInvoiceId);
        ReturnPurchaseInvoiceDetail GetReturnPurchaseInvoiceDetailForEdit(Guid returnPurchaseInvoiceDetailId);
    }
}
