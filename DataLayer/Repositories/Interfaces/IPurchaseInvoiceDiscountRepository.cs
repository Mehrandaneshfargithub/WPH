using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPurchaseInvoiceDiscountRepository : IRepository<PurchaseInvoiceDiscount>
    {
        IEnumerable<PurchaseInvoiceDiscount> GetAllPurchaseInvoiceDiscounts(Guid purchaseInvoiceId);
    }
}
