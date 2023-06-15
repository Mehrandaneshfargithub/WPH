using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPurchaseInvoiceRepository : IRepository<PurchaseInvoice>
    {
        IEnumerable<PurchaseInvoice> GetAllPurchaseInvoiceByInvoiceNum(Guid clinicSectionId, string invoiceNum);
        IEnumerable<PurchaseInvoice> GetAllPurchaseInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<PurchaseInvoice, bool>> predicate = null);
        PurchaseInvoice GetPurchaseInvoice(Guid purchaseInvoiceId);
        string GetLatestPurchaseInvoiceNum(Guid clinicSectionId);
        IEnumerable<PurchaseInvoice> GetAllPurchaseInvoiceFroReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo, bool detail);
        PurchaseInvoice GetForUpdateTotalPrice(Guid purchaseInvoiceId);
        bool CheckRepeatedInvoiceNum(Guid supplierId, string mainInvoiceNum, Expression<Func<PurchaseInvoice, bool>> predicate = null);
        IEnumerable<PartialPayModel> GetNotPartialPayPurchaseInvoice(Guid? supplierId, int? currencyId, Guid? payId);
        IEnumerable<PartialPayModel> GetPartialPayPurchaseInvoice(Guid? payId, int? currencyId);
        IEnumerable<PurchaseInvoice> GetNotPayPurchaseInvoice(Guid? supplierId);
        IEnumerable<PurchaseInvoice> GetPayPurchaseInvoice(Guid? payId);
        IEnumerable<PurchaseInvoice> GetForPay(IEnumerable<Guid> purchaseIds);
        PurchaseInvoice GetPurchaseForReport(Guid purchaseInvoiceId);
    }
}
