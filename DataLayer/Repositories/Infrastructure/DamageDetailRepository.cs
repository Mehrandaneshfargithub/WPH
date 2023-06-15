using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class DamageDetailRepository : Repository<DamageDetail>, IDamageDetailRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public DamageDetailRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<DamageDetail> GetAllDamageDetailByMasterId(Guid damageId)
        {
            return Context.DamageDetails.AsNoTracking()
                .Include(p => p.Product).ThenInclude(P => P.ProductType)
                .Include(p => p.Product).ThenInclude(P => P.Producer)
                .Include(p => p.Currency)
                .Where(p => p.MasterId == damageId)
                ;
        }

        public IEnumerable<DamageDetail> GetAllTotalPrice(Guid damageId)
        {
            return Context.DamageDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Where(p => p.MasterId == damageId)
                .Select(p => new DamageDetail
                {
                    Num = p.Num,
                    Price = p.Price,
                    Discount = p.Discount,
                    Currency = new BaseInfoGeneral
                    {
                        Name = p.Currency.Name
                    }
                })
                ;
        }

        public DamageDetail GetDamageDetailForEdit(Guid damageDetailId)
        {
            return _context.DamageDetails.AsNoTracking()
                .Include(p => p.Currency)
                .Include(p => p.PurchaseInvoiceDetail).ThenInclude(p => p.Master)
                .SingleOrDefault(p => p.Guid == damageDetailId);
        }

        public IEnumerable<DamageDetail> GetByMultipleIds(List<Guid> details)
        {
            return _context.DamageDetails.AsNoTracking()
                .Where(p => details.Contains(p.Guid));
        }

    }
}
