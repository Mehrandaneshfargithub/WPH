using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISaleInvoiceRepository : IRepository<SaleInvoice>
    {
        IEnumerable<SaleInvoice> GetAllSaleInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? customerId, string invoiceNum, Guid? productId);
        SaleInvoice GetWithType(Guid saleInvoiceId);
        string GetLatestSaleInvoiceNum(Guid clinicSectionId);
        SaleInvoice GetForUpdateTotalPrice(Guid saleInvoiceId);
        SaleInvoice CheckForCurrency(Guid saleInvoiceId, int? currencyId);
        IEnumerable<SaleInvoice> GetForReceive(IEnumerable<Guid> receiveIds);
        IEnumerable<SaleInvoice> GetReceiveSaleInvoice(Guid? receiveId);
        IEnumerable<SaleInvoice> GetNotReceiveSaleInvoice(Guid? customerId);
        void RemoveSaleInvoice(Guid saleInvoiceid);
        IEnumerable<IncomeModel> GetAllIncomes(Guid clinicSectionId);
        SaleInvoice GetByReceiveId(Guid receiveId);
        IEnumerable<IncomeModel> GetAllStoreInCome(Guid clinicSectionId);
    }
}
