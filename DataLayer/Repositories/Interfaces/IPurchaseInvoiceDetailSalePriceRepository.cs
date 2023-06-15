using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPurchaseInvoiceDetailSalePriceRepository : IRepository<PurchaseInvoiceDetailSalePrice>
    {
        PurchaseInvoiceDetailSalePrice GetForTransferEdit(Guid salePriceId);
        PurchaseInvoiceDetailSalePrice GetForEdit(Guid salePriceId);
        bool CheckConflictCurrency(Guid? purchaseInvoiceDetailId, int? currencyId);
        bool CheckCurrencyExist(Guid? purchaseInvoiceDetailId, int? currencyId, int? typeId, Expression<Func<PurchaseInvoiceDetailSalePrice, bool>> predicate = null);
        public bool CheckTransferCurrencyExist(Guid? transferDetailId, int? currencyId, int? typeId, Expression<Func<PurchaseInvoiceDetailSalePrice, bool>> predicate = null);
        IEnumerable<PurchaseInvoiceDetailSalePrice> GetAllTransferDetailSalePrice(Guid transferDetailId);
        IEnumerable<PurchaseInvoiceDetailSalePrice> GetAllPurchaseInvoiceDetailSalePrice(Guid purchaseInvoiceDetailId);
    }
}
