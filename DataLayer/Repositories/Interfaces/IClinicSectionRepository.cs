using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IClinicSectionRepository : IRepository<ClinicSection>
    {
        List<ClinicSection> GetClinicSectionsByIds(List<Guid> clinicSectionIds);
        ClinicSection GetWithSectionName(Guid clinicSectionId);
        IEnumerable<DashboardPoco> GetAllDashboardDatas(Guid clinicSectionId);
        void UpdateClinicSectionName(Guid clinicSectionId, string name);
        IEnumerable<ClinicSection> GetAllClinicSectionWithType(Guid clinicId);
        IEnumerable<Guid> GetClinicSectionChilds(List<Guid> clinicSections, Guid? UserId);
        bool ClinicSectionHasChild(Guid clinicSectionId, Guid UserId);
        IEnumerable<ClinicSection> GetAllClinicSectionsChild(Guid clinicSectionId, Guid UserId);
        IEnumerable<ClinicSection> GetAllParentClinicSections();
        IEnumerable<ClinicSection> GetAllClinicSectionsBySectionTypeId(Expression<Func<ClinicSection, bool>> predicate = null);
        IEnumerable<ClinicSection> GetClinicSectionParents();
        bool CheckNameExists(string name, int? clinicSectionTypeId, Expression<Func<ClinicSection, bool>> predicate = null);
        IEnumerable<ClinicSection> GetAllMainClinicSectionsExceptOne(Guid clinicSectionId);
        IEnumerable<ClinicSection> GetAllClinicSectionsChildForTransferSource(Guid clinicSectionId, Guid userId);
        bool CheckClinicSectionIsParent(Guid sourceClinicSectionId, Guid destinationClinicSectionId);
        IEnumerable<ClinicSection> GetAllClinicSectionsWithChilds(Guid clinicId);
        IEnumerable<ClinicSection> GetAllAccessedUserClinicSectionWithChilds(Guid userId);
    }
}
