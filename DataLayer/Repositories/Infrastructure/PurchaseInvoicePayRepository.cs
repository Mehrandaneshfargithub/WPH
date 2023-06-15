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
    public class PurchaseInvoicePayRepository : Repository<PurchaseInvoicePay>, IPurchaseInvoicePayRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PurchaseInvoicePayRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckRepeatedPay(IEnumerable<Guid> purchaseIds, Guid? payId)
        {
            return Context.PurchaseInvoicePays.AsNoTracking()
                .Include(p => p.Pay)
                .Where(p => purchaseIds.Contains(p.InvoiceId.Value) && (payId == null || p.PayId != payId))
                .Any()
                ;
        }

        public bool CheckPurchaseInvoicePaid(Guid purchaseInvoiceId)
        {
            return _context.PurchaseInvoicePays.AsNoTracking()
                .Where(p => p.InvoiceId == purchaseInvoiceId)
                .Any();
        }
    }
}
