using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReturnPurchaseInvoicePayRepository : Repository<ReturnPurchaseInvoicePay>, IReturnPurchaseInvoicePayRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnPurchaseInvoicePayRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckRepeatedPay(IEnumerable<Guid> purchaseIds, Guid? payId)
        {
            return Context.ReturnPurchaseInvoicePays.AsNoTracking()
                .Include(p => p.Pay)
                .Where(p => purchaseIds.Contains(p.InvoiceId.Value) && (payId == null || p.PayId != payId))
                .Any()
                ;
        }

        public bool CheckReturnPurchaseInvoicePaid(Guid purchaseInvoiceId)
        {
            return _context.ReturnPurchaseInvoicePays.AsNoTracking()
                .Where(p => p.InvoiceId == purchaseInvoiceId)
                .Any();
        }
    }
}
