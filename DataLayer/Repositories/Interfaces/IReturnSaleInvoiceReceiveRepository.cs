using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReturnSaleInvoiceReceiveRepository : IRepository<ReturnSaleInvoiceReceive>
    {
        bool CheckRepeatedReceive(IEnumerable<Guid> saleIds, Guid? receiveId);
        bool CheckReturnSaleInvoiceReceived(Guid saleInvoiceId);
    }
}
