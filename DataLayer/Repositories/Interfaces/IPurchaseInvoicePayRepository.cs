using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IPurchaseInvoicePayRepository : IRepository<PurchaseInvoicePay>
    {
        bool CheckRepeatedPay(IEnumerable<Guid> purchaseIds, Guid? payId);
        bool CheckPurchaseInvoicePaid(Guid purchaseInvoiceId);
    }
}
