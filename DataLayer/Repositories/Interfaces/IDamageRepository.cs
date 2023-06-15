using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IDamageRepository : IRepository<Damage>
    {
        IEnumerable<Damage> GetAllDamage(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<Damage, bool>> predicate = null);
        Damage GetDamage(Guid damageId);
        string GetLatestDamageNum(Guid clinicSectionId);
        Damage GetForUpdateTotalPrice(Guid damageId);
    }
}
