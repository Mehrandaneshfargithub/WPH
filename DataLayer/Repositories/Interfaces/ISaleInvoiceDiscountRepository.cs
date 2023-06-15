using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISaleInvoiceDiscountRepository : IRepository<SaleInvoiceDiscount>
    {
        IEnumerable<SaleInvoiceDiscount> GetAllSaleInvoiceDiscounts(Guid purchaseInvoiceId);
        SaleInvoiceDiscount GetSaleInvoiceDiscountByCurrencyId(Guid saleInvoiceId, int currencyId);
    }
}
