using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class UserSubSystemAccessRepository : Repository<UserSubSystemAccess>, IUserSubSystemAccessRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public UserSubSystemAccessRepository(WASContext context)
            : base(context)
        {
        }

        public void SaveAccessForUser(Guid userId, List<UserSubSystemAccess> userSubSystemAccess, Guid clinicSectionId)
        {
            try
            {
                _context.UserSubSystemAccesses.RemoveRange(_context.UserSubSystemAccesses.Where(x => x.UserId == userId && x.ClinicSectionId == clinicSectionId));
                _context.UserSubSystemAccesses.AddRange(userSubSystemAccess);
                _context.SaveChanges();

            }
            catch (Exception e) { throw e; }
        }

        public bool CheckUserAccess(Guid userId, Guid clinicSectionId, string accessName, string subSystemName)
        {
            return _context.UserSubSystemAccesses.AsNoTracking()
                .Include(p => p.SubSystemAccess.Access)
                .Include(p => p.SubSystemAccess.SubSystem)
                .Where(p => p.UserId == userId && p.ClinicSectionId == clinicSectionId && p.SubSystemAccess.Access.Name == accessName && p.SubSystemAccess.SubSystem.Name == subSystemName)
                .Any();
        }

        public IEnumerable<SubSystemAccess> GetUserSubSystemAccess(Guid userId, Guid clinicSectionId, string[] subSystemsName)
        {
            return _context.UserSubSystemAccesses.AsNoTracking()
                .Include(p => p.SubSystemAccess.Access)
                .Include(p => p.SubSystemAccess.SubSystem)
                .Where(p => p.UserId == userId && p.ClinicSectionId == clinicSectionId &&  subSystemsName.Contains(p.SubSystemAccess.SubSystem.Name))
                .Select(p => new SubSystemAccess
                {
                    SubSystem = new SubSystem
                    {
                        Name = p.SubSystemAccess.SubSystem.Name
                    },
                    Access = new Access
                    {
                        Name = p.SubSystemAccess.Access.Name
                    }
                });
        }
    }
}
