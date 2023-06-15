using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Infrastracture
{
    public class DMSCustomerRepository : Repository<TblCustomer>, IDMSCustomerRepository
    {
        protected readonly DMSContext _Context;

        public DMSCustomerRepository(DMSContext Context) : base(Context)
        {
            _Context = Context;
        }

        public int GetCustomerIdByName(string Name)
        {
            try
            {
                return _Context.TblCustomers.SingleOrDefault(x => x.Name == Name).Id;
            }
            catch
            {
                return 0;
            }
        }
    }
}
