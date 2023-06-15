using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class DamageRepository : Repository<Damage>, IDamageRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public DamageRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Damage> GetAllDamage(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<Damage, bool>> predicate = null)
        {
            IQueryable<Damage> result = _context.Damages.AsNoTracking()
                .Include(p => p.Reason)
                .Include(p => p.CostType)
                .Include(p => p.DamageDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.ClinicSectionId == clinicSectionId && p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new Damage
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                TotalPrice = p.TotalPrice,
                DamageDiscounts = (ICollection<DamageDiscount>)p.DamageDiscounts.Select(x => new DamageDiscount
                {
                    Amount = x.Amount,
                    CurrencyName = x.Currency.Name
                }),
                CostType = new BaseInfo
                {
                    Name = p.CostType.Name
                },
                Reason = new BaseInfo
                {
                    Name = p.Reason.Name
                }
            });
        }

        public Damage GetDamage(Guid purchaseInvoiceId)
        {
            return _context.Damages.AsNoTracking()
                .Include(p => p.DamageDetails).ThenInclude(p => p.Currency)
                .Include(p => p.DamageDiscounts).ThenInclude(p => p.Currency)
                .Include(p => p.CostType)
                .Include(p => p.Reason)
                .Select(p => new Damage
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    Description = p.Description,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    ReasonId = p.ReasonId,
                    CostTypeId = p.CostTypeId,
                    DamageDetails = (ICollection<DamageDetail>)p.DamageDetails.Select(x => new DamageDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        Price = x.Price,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    DamageDiscounts = (ICollection<DamageDiscount>)p.DamageDiscounts.Select(x => new DamageDiscount
                    {
                        Guid = x.Guid,
                        Amount = x.Amount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                    }),
                    Reason = new BaseInfo
                    {
                        Name = p.Reason.Name
                    },
                    CostType = new BaseInfo
                    {
                        Name = p.CostType.Name
                    }
                })
                .SingleOrDefault(p => p.Guid == purchaseInvoiceId);
        }

        public string GetLatestDamageNum(Guid clinicSectionId)
        {
            try
            {
                return _context.FN_LatestDamageNum(clinicSectionId).FirstOrDefault().CODE;
            }
            catch (Exception e) { throw e; }

        }

        public Damage GetForUpdateTotalPrice(Guid purchaseInvoiceId)
        {
            return _context.Damages.AsNoTracking()
                .Include(p => p.DamageDetails).ThenInclude(p => p.Currency)
                .Include(p => p.DamageDiscounts).ThenInclude(p => p.Currency)
                .Where(p => p.Guid == purchaseInvoiceId)
                .Select(p => new Damage
                {
                    Guid = p.Guid,
                    InvoiceNum = p.InvoiceNum,
                    InvoiceDate = p.InvoiceDate,
                    Description = p.Description,
                    CreatedUserId = p.CreatedUserId,
                    CreateDate = p.CreateDate,
                    ModifiedUserId = p.ModifiedUserId,
                    ModifiedDate = p.ModifiedDate,
                    ClinicSectionId = p.ClinicSectionId,
                    TotalPrice = p.TotalPrice,
                    ReasonId = p.ReasonId,
                    CostTypeId = p.CostTypeId,
                    DamageDetails = (ICollection<DamageDetail>)p.DamageDetails.Select(x => new DamageDetail
                    {
                        Guid = x.Guid,
                        Num = x.Num,
                        Price = x.Price,
                        Discount = x.Discount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                        //Currency = new BaseInfoGeneral
                        //{
                        //    Name = x.Currency.Name
                        //}
                    }),
                    DamageDiscounts = (ICollection<DamageDiscount>)p.DamageDiscounts.Select(x => new DamageDiscount
                    {
                        Guid = x.Guid,
                        Amount = x.Amount,
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.Currency.Name,
                        //Currency = new BaseInfoGeneral
                        //{
                        //    Name = x.Currency.Name
                        //}
                    })
                }).SingleOrDefault();
        }

    }
}
