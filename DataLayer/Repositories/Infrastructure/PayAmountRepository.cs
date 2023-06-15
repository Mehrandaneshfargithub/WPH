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
    public class PayAmountRepository : Repository<PayAmount>, IPayAmountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public PayAmountRepository(WASContext context)
            : base(context)
        {
        }

    }
}
