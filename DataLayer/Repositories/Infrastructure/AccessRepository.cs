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
    public class AccessRepository : Repository<Access>, IAccessRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AccessRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Access> GetSubsystemAccess(int subSystemId)
        {
            return _context.Accesses.AsNoTracking()
                .Include(p => p.SubSystemAccesses)
                .Select(p => new Access
                {
                    Id = p.Id,
                    Name = p.Name,
                    SubSystemAccesses = (ICollection<SubSystemAccess>)p.SubSystemAccesses.Where(x => x.SubSystemId == subSystemId).Select(s => new SubSystemAccess
                    {
                        AccessId = s.AccessId,
                        SubSystemId = s.SubSystemId
                    })
                });
        }
    }
}
