using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IChildRepository : IRepository<Child>
    {
        Child GetChildWithUser(Guid childId);
        IEnumerable<Child> GetAllChild(Guid clinicSectionId);
        IEnumerable<Child> GetAllUnknownChildren(Guid clinicSectionId);
        IEnumerable<Child> GetAllHospitalPatientChildren(Guid receptionId);
        IEnumerable<Child> GetDetailChildReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<Child, bool>> predicate = null);
    }
}
