using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAllClinicSectionUsersExept(string excludedUsertype, Guid ClinicSectionId, Expression<Func<User, bool>> predicate = null);
        User GetUserByName(string name, Guid clinicId);
        void UpdateActivationStatus(User user);
        void UpdateUserExceptPass(User user);
        User GetUserBasedOnUserName(Guid clinicSectionId, string userName);
        User GetUserByUserNameAndPass(string userName, string password, Guid clinicId, int? userId);
        int GetUserCountByUserNameAndPass(string userName, string password, Guid clinicId, int? userId);
        User GetUserWithClinicSections(Guid userId);
        IEnumerable<ClinicSectionUser> GetAllClinicsUsers(Guid clinicId);
        User CheckUserExistBaseOnUserName(Guid ClinicId, string userName, string currentUserName = "");
        User CheckUserExistBaseOnName(Guid clinicSectionId, string Name, string currentName = "");
        Guid GetUserIdByUserName(Guid clinicSectionId, string userName);
        User GetUserWithAccess(Guid userId);
        void RemoveUser(Guid userId);
        Guid UpdateUser(User userDto);
        User GetUserWithRole(Guid userId);
        User CheckAdminLogin(string userName, string password);
        bool CheckUserByIdAndPass(Guid userId, string pass);
        IEnumerable<User> GetAllUsersExeptUserPortions(Guid clinicSectionId);
    }
}
