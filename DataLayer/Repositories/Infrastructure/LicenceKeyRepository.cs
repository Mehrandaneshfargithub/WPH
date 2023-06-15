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
    public class LicenceKeyRepository : Repository<LicenceKey>, ILicenceKeyRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public LicenceKeyRepository(WASContext context)
            : base(context)
        {
        }

        public LicenceKey GetLastLicence(string computerSerial)
        {
            return _context.LicenceKeys.AsNoTracking()
                .Where(p => p.ComputerSerial == computerSerial)
                .OrderBy(p => p.Id)
                .LastOrDefault();
        }
    }
}
