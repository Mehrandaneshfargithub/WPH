using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class DamageDiscountRepository : Repository<DamageDiscount>, IDamageDiscountRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public DamageDiscountRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<DamageDiscount> GetAllDamageDiscounts(Guid damageId)
        {
            return _context.DamageDiscounts.AsNoTracking()
            .Include(x => x.Currency)
            .Where(x => x.DamageId == damageId)
            .Select(p => new DamageDiscount
            {
                Guid = p.Guid,
                Amount = p.Amount,
                Description = p.Description,
                Currency = new BaseInfoGeneral
                {
                    Name = p.Currency.Name
                }
            });
        }
    }
}
