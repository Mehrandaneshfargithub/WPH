using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReturnPurchaseInvoiceRepository : IRepository<ReturnPurchaseInvoice>
    {
        IEnumerable<ReturnPurchaseInvoice> GetAllReturnPurchaseInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<ReturnPurchaseInvoice, bool>> predicate = null);
        ReturnPurchaseInvoice GetForUpdateTotalPrice(Guid returnPurchaseInvoiceId);
        string GetLatestReturnPurchaseInvoiceNum(Guid clinicSectionId);
        IEnumerable<PartialPayModel> GetNotPartialPayReturnPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId);
        IEnumerable<PartialPayModel> GetPartialPayReturnPurchaseInvoice(Guid? payId);
        IEnumerable<ReturnPurchaseInvoice> GetNotPayReturnPurchaseInvoice(Guid? supplierId);
        IEnumerable<ReturnPurchaseInvoice> GetPayReturnPurchaseInvoice(Guid? payId);
        IEnumerable<ReturnPurchaseInvoice> GetForPay(IEnumerable<Guid> returnIds);
        ReturnPurchaseInvoice GetReturnPurchaseInvoice(Guid returnPurchaseInvoiceId);
    }
}
