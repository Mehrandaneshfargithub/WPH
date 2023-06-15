using DMSDataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Interfaces
{
    public interface IDMSSaleInvoiceRepository : IRepository<TblSaleInvoice>
    {
        IEnumerable<FN_MedicineNumModel> GetAllSaleProductByCustomerId(int customerId);
    }
}
