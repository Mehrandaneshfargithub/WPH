using DataLayer.EntityModels;
using System;
using System.Collections.Generic;

namespace DataLayer.Repositories.Interfaces
{
    public interface IDamageDetailRepository : IRepository<DamageDetail>
    {
        IEnumerable<DamageDetail> GetAllDamageDetailByMasterId(Guid clinicSectionId);
        IEnumerable<DamageDetail> GetAllTotalPrice(Guid damageId);
        DamageDetail GetDamageDetailForEdit(Guid damageDetailId);
        IEnumerable<DamageDetail> GetByMultipleIds(List<Guid> details);
    }
}
