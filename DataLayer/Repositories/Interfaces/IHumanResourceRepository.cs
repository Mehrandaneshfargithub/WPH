using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IHumanResourceRepository : IRepository<HumanResource>
    {
        IEnumerable<HumanResource> GetAllHuman(List<Guid> sections, Expression<Func<HumanResource, bool>> predicate = null);
        IEnumerable<HumanResource> GetAllTreatmentStaff(List<Guid> sections, Expression<Func<HumanResource, bool>> predicate = null);
        HumanResource GetHumanById(Guid? humanId);
        HumanResource GetHumanByName(string humanName);

        HumanResource GetById(Guid? humanId);
    }
}
