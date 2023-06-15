using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISaleInvoiceReceiveRepository : IRepository<SaleInvoiceReceive>
    {
        bool CheckRepeatedReceive(IEnumerable<Guid> saleIds, Guid? receiveId);
        bool CheckSaleInvoiceInUse(Guid saleInvoiceId);
    }
}
