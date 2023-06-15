using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class SubSystemRepository : Repository<SubSystem>, ISubSystemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public SubSystemRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<SubSystem> GetAllUserSubSystems(Guid userId, int sectionTypeId, Guid clinicSectionId, int LanguageId)
        {
            try
            {
                return _context.FN_GetAllUserSubsystems(userId, sectionTypeId, clinicSectionId, LanguageId).ToList();
            }
            catch (Exception e) { throw e; }
        }
        public IEnumerable<AllSubSystemsWithAccess> GetAllSubSystemWithAccess(Guid userId, int SectionTypeId, Guid clinicSectionId, int LanguageId, Guid parentUserId)
        {
            return _context.FN_GetAllSubSystemsWithAccess(userId, SectionTypeId, clinicSectionId, LanguageId, parentUserId);
        }

        public IEnumerable<SubSystem> GetAllSubsystemsWithParent()
        {
            return _context.SubSystems.AsNoTracking()
                .Include(p => p.ParentRelation);
        }

        public IEnumerable<SubSystem> GetSubsystemParents()
        {
            return _context.SubSystems.AsNoTracking()
                .Where(p => p.ParentRelationId == null)
                .Select(p => new SubSystem
                {
                    Id = p.Id,
                    Name = p.Name
                });
        }

        public bool CheckNameExists(string name, Expression<Func<SubSystem, bool>> predicate = null)
        {
            IQueryable<SubSystem> result = _context.SubSystems.AsNoTracking()
                .Where(p => p.Name == name);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<SubSystemAccess> GetSubSystemAccessBySubSystemId(int subSystemId)
        {
            return _context.SubSystemAccesses.AsNoTracking()
                .Include(p => p.UserSubSystemAccesses)
                .Where(p => p.SubSystemId == subSystemId);
        }

        public void RemoveRangeSubSystemAccess(List<SubSystemAccess> subSystemAccesses)
        {
            _context.SubSystemAccesses.RemoveRange(subSystemAccesses);
        }

        public void AddRangeSubSystemAccess(List<SubSystemAccess> subSystemAccesses)
        {
            _context.SubSystemAccesses.AddRange(subSystemAccesses);
        }

    }
}
