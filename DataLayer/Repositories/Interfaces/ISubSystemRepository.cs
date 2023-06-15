using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface ISubSystemRepository : IRepository<SubSystem>
    {
        IEnumerable<SubSystem> GetAllUserSubSystems(Guid userId, int sectionTypeId, Guid clinicSectionId, int LanguageId);
        IEnumerable<AllSubSystemsWithAccess> GetAllSubSystemWithAccess(Guid userId, int SectionTypeId, Guid clinicSectionId, int LanguageId, Guid parentUserId);
        IEnumerable<SubSystem> GetAllSubsystemsWithParent();
        IEnumerable<SubSystem> GetSubsystemParents();
        bool CheckNameExists(string name, Expression<Func<SubSystem, bool>> predicate = null);
        IEnumerable<SubSystemAccess> GetSubSystemAccessBySubSystemId(int subSystemId);
        void RemoveRangeSubSystemAccess(List<SubSystemAccess> subSystemAccesses);
        void AddRangeSubSystemAccess(List<SubSystemAccess> subSystemAccesses);
    }
}
