using DMSDataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Interfaces
{
    public interface IDMSCustomerRepository : IRepository<TblCustomer>
    {
        int GetCustomerIdByName(string Name);
    }
}
