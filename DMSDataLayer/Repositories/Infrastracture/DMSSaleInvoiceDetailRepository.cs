using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Infrastracture
{
    public class DMSSaleInvoiceDetailRepository : Repository<TblSaleInvoiceDetail>, IDMSSaleInvoiceDetailRepository
    {
        protected readonly DMSContext _Context;

        public DMSSaleInvoiceDetailRepository(DMSContext Context) : base(Context)
        {
            _Context = Context;
        }
    }
}
