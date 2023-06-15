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
    public class SaleInvoiceReceiveRepository : Repository<SaleInvoiceReceive>, ISaleInvoiceReceiveRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SaleInvoiceReceiveRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckRepeatedReceive(IEnumerable<Guid> saleIds, Guid? receiveId)
        {
            return Context.SaleInvoiceReceives.AsNoTracking()
                .Include(p => p.Receive)
                .Where(p => saleIds.Contains(p.InvoiceId.Value) && (receiveId == null || p.ReceiveId != receiveId))
                .Any()
                ;
        }

        public bool CheckSaleInvoiceInUse(Guid saleInvoiceId)
        {
            return _context.SaleInvoiceReceives.AsNoTracking()
                .Where(p => p.InvoiceId == saleInvoiceId)
                .Any();
        }
    }
}
