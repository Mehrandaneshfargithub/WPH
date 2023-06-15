using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IUserSubSystemAccessRepository : IRepository<UserSubSystemAccess>
    {
        void SaveAccessForUser(Guid userId, List<UserSubSystemAccess> userSubSystemAccess, Guid clinicSectionId);
        bool CheckUserAccess(Guid userId, Guid clinicSectionId, string accessName, string subSystemName);
        IEnumerable<SubSystemAccess> GetUserSubSystemAccess(Guid userId, Guid clinicSectionId, string[] subSystemsName);
    }
}
