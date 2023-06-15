using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Infrastructure
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public UserRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<User> GetAllClinicSectionUsersExept(string excludedUsertype, Guid ClinicSectionId, Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<User> result = _context.Users.AsNoTracking().Include(p => p.UserType);
            if (predicate != null)
                result = result.Where(predicate);

            return result.Where(x => x.UserName != "developer" && (x.IsUser != null && x.IsUser.Value) && x.ClinicSectionId == ClinicSectionId && x.UserType.Name != null && x.UserType.Name != excludedUsertype);

        }

        public User GetUserByName(string name, Guid clinicSectionId)
        {
            return _context.Users.AsNoTracking().Where(x => x.Name == name && x.ClinicSectionId == clinicSectionId).FirstOrDefault();
        }
        public void UpdateActivationStatus(User user)
        {
            Context.Entry(user).State = EntityState.Modified;
            Context.Entry(user).Property(x => x.Active).IsModified = true;
        }
        public void UpdateUserExceptPass(User user)
        {
            Context.Entry(user).State = EntityState.Modified;
            //Context.Users.Attach(user);
            //Context.Entry(user).State = EntityState.Modified;
            //Context.Entry(user).Property(x => x.Pass1).IsModified = false;
        }

        public User GetUserBasedOnUserName(Guid clinicSectionId, string userName)
        {
            if (userName != "developer")
            {
                return _context.Users.AsNoTracking()
               .Include(x => x.ClinicSectionUsers).ThenInclude(y => y.ClinicSection)
               .Where(x => x.UserName == userName && x.ClinicSectionUsers.Any(s => s.ClinicSectionId == clinicSectionId)).First();
            }
            else
            {
                return _context.Users.AsNoTracking()
               .Where(x => x.UserName == userName).First();
            }

        }

        public User GetUserByUserNameAndPass(string userName, string password, Guid clinicId, int? userId)
        {
            IQueryable<User> result = _context.Users.AsNoTracking()
                .Include(x => x.AccessType)
                .Include(x => x.ClinicSectionUsers)
                .Where(x => x.UserName == userName && x.Pass3 == password && x.Active == true);

            if (clinicId != Guid.Empty)
                result = result.Where(x => x.ClinicSectionId == clinicId);

            if (userId != null)
                result = result.Where(x => x.Id == userId);

            return result.SingleOrDefault();
        }

        public int GetUserCountByUserNameAndPass(string userName, string password, Guid clinicId, int? userId)
        {
            IQueryable<User> result = _context.Users.AsNoTracking()
                .Include(x => x.AccessType)
                .Include(x => x.ClinicSectionUsers)
                .Where(x => x.UserName == userName && x.Pass3 == password && x.Active == true);

            if (clinicId != Guid.Empty)
                result = result.Where(x => x.ClinicSectionId == clinicId);

            if (userId != null)
                result = result.Where(x => x.Id == userId);

            return result.Count();
        }

        public User CheckAdminLogin(string userName, string password)
        {
            return _context.Users.AsNoTracking()
                .Include(p => p.AccessType)
                .Where(p => p.UserName == userName && p.Pass1 == password && p.AccessType.Name == "ClinicAdmin")
                .SingleOrDefault();
        }

        public User GetUserWithClinicSections(Guid userId)
        {
            return _context.Users.AsNoTracking().Include(x => x.ClinicSectionUsers).SingleOrDefault(x => x.Guid == userId);
        }

        public IEnumerable<ClinicSectionUser> GetAllClinicsUsers(Guid clinicId)
        {
            return _context.ClinicSectionUsers.AsNoTracking().Include(x => x.ClinicSection).Include(x => x.User.UserType)
                .Where(x => x.User.ClinicSectionId == clinicId && x.User.AccessTypeId != null);
        }

        public User CheckUserExistBaseOnUserName(Guid ClinicSectionId, string userName, string currentUserName = "")
        {
            return _context.Users.AsNoTracking().Where(x => x.UserName == userName && x.UserName != currentUserName && x.ClinicSectionId == ClinicSectionId).FirstOrDefault();
        }

        public User CheckUserExistBaseOnName(Guid clinicSectionId, string Name, string currentName = "")
        {
            return _context.Users.AsNoTracking()
                .Where(x => x.Name == Name && x.Name != currentName && x.ClinicSectionId == clinicSectionId)
                .FirstOrDefault();
        }

        public Guid GetUserIdByUserName(Guid clinicSectionId, string userName)
        {
            if (userName != "developer")
            {
                return _context.Users.AsNoTracking()
               .Include(x => x.ClinicSectionUsers).ThenInclude(y => y.ClinicSection)
               .Where(x => x.UserName == userName && x.ClinicSectionUsers.Any(s => s.ClinicSectionId == clinicSectionId)).First().Guid;
            }
            else
            {
                return _context.Users.AsNoTracking()
               .Where(x => x.UserName == userName).First().Guid;
            }
        }

        public User GetUserWithAccess(Guid userId)
        {
            return _context.Users.AsNoTracking().Include(x => x.AccessType).SingleOrDefault(x => x.Guid == userId);
        }

        public void RemoveUser(Guid userId)
        {
            _context.UserProfiles.RemoveRange(_context.UserProfiles.Where(x => x.UserId == userId));
            _context.UserSubSystemAccesses.RemoveRange(_context.UserSubSystemAccesses.Where(x => x.UserId == userId));
            _context.ClinicSectionUsers.RemoveRange(_context.ClinicSectionUsers.Where(x => x.UserId == userId));
            _context.Users.Remove(_context.Users.SingleOrDefault(x => x.Guid == userId));
        }

        public Guid UpdateUser(User userDto)
        {
            try
            {
                _context.ClinicSectionUsers.RemoveRange(_context.ClinicSectionUsers.Where(x => x.UserId == userDto.Guid));
                _context.ClinicSectionUsers.AddRange(userDto.ClinicSectionUsers);
                _context.Users.Update(userDto);
                _context.SaveChanges();
                return userDto.Guid;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public User GetUserWithRole(Guid userId)
        {
            return _context.Users.AsNoTracking().Include(x => x.UserType).SingleOrDefault(x => x.Guid == userId);
        }

        public bool CheckUserByIdAndPass(Guid userId, string pass)
        {
            return _context.Users.AsNoTracking()
                .Where(p => p.Guid == userId && p.Pass3 == pass && p.Active == true)
                .Any();
        }

        public IEnumerable<User> GetAllUsersExeptUserPortions(Guid clinicSectionId)
        {
            //return _context.Users.AsNoTracking().Where(a => a.Pass3 != null).Select(a => a.Guid).Except(_context.UserPortions.Select(a => a.UserId));
            return _context.Users.AsNoTracking().Where(a => a.Pass3 != null && a.ClinicSectionId == clinicSectionId &&
                !_context.UserPortions.Select(x => x.Guid).Contains(a.Guid));
        }
    }
}
