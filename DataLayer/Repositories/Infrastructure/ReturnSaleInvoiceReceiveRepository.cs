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
    public class ReturnSaleInvoiceReceiveRepository : Repository<ReturnSaleInvoiceReceive>, IReturnSaleInvoiceReceiveRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReturnSaleInvoiceReceiveRepository(WASContext context)
            : base(context)
        {
        }

        public bool CheckRepeatedReceive(IEnumerable<Guid> saleIds, Guid? receiveId)
        {
            return Context.ReturnSaleInvoiceReceives.AsNoTracking()
                .Include(p => p.Receive)
                .Where(p => saleIds.Contains(p.InvoiceId.Value) && (receiveId == null || p.ReceiveId != receiveId))
                .Any()
                ;
        }

        public bool CheckReturnSaleInvoiceReceived(Guid saleInvoiceId)
        {
            return _context.ReturnSaleInvoiceReceives.AsNoTracking()
                .Where(p => p.InvoiceId == saleInvoiceId)
                .Any();
        }
    }
}
