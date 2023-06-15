using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class MoneyConvertRepository : Repository<MoneyConvert>, IMoneyConvertRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public MoneyConvertRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<MoneyConvert> GetAllMoneyConvertByDate(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {
            return Context.MoneyConverts.AsNoTracking()
              .Include(x => x.BaseCurrency)
              .Include(x => x.DestCurrency)
              .Where(x => x.IsMain != null && x.IsMain.Value && x.Date <= dateTo && x.Date >= dateFrom && x.ClinicSectionId == clinicSectionId);

        }

        public void RemoveMoneyConvert(Guid moneyConvertId)
        {
            MoneyConvert se = new MoneyConvert() { Guid = moneyConvertId };
            Context.Entry(se).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public decimal? GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            try
            {
                return Context.FN_GetMoneyConvertAmuont(clinicSectionId, baseCurrencyId, destCurrencyId, 1).FirstOrDefault().Remain;
            }
            catch { return -1; }
        }

        public MoneyConvert GetMoneyConvertBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => p.IsMain != null && p.IsMain.Value && p.ClinicSectionId == clinicSectionId && ((p.BaseCurrencyId == baseCurrencyId && p.DestCurrencyId == destCurrencyId) || (p.BaseCurrencyId == destCurrencyId && p.DestCurrencyId == baseCurrencyId)))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .Select(p => new MoneyConvert
                {
                    BaseAmount = p.BaseAmount,
                    BaseCurrencyId = p.BaseCurrencyId,
                    BaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.BaseCurrency.Name
                    },
                    DestAmount = p.DestAmount,
                    DestCurrencyId = p.DestCurrencyId,
                    DestCurrency = new BaseInfoGeneral
                    {
                        Name = p.DestCurrency.Name
                    }
                })
                .FirstOrDefault();
        }

        public IEnumerable<MoneyConvert> GetLatestMoneyConverts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => p.IsMain != null && p.IsMain.Value && p.ClinicSectionId == clinicSectionId && ((p.BaseCurrencyId == baseCurrencyId && p.DestCurrencyId == destCurrencyId) || (p.BaseCurrencyId == destCurrencyId && p.DestCurrencyId == baseCurrencyId)))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .Select(p => new MoneyConvert
                {
                    Guid = p.Guid,
                    BaseAmount = p.BaseAmount,
                    BaseCurrencyId = p.BaseCurrencyId,
                    BaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.BaseCurrency.Name
                    },
                    DestAmount = p.DestAmount,
                    DestCurrencyId = p.DestCurrencyId,
                    DestCurrency = new BaseInfoGeneral
                    {
                        Name = p.DestCurrency.Name
                    }
                })
                .Take(20);
        }

        public IEnumerable<MoneyConvert> GetLatestMoneyConvertsWithIsMain(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => p.ClinicSectionId == clinicSectionId && ((p.BaseCurrencyId == baseCurrencyId && p.DestCurrencyId == destCurrencyId) || (p.BaseCurrencyId == destCurrencyId && p.DestCurrencyId == baseCurrencyId)))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .Select(p => new MoneyConvert
                {
                    Guid = p.Guid,
                    BaseAmount = p.BaseAmount,
                    BaseCurrencyId = p.BaseCurrencyId,
                    BaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.BaseCurrency.Name
                    },
                    DestAmount = p.DestAmount,
                    DestCurrencyId = p.DestCurrencyId,
                    DestCurrency = new BaseInfoGeneral
                    {
                        Name = p.DestCurrency.Name
                    }
                })
                .Take(20);
        }

        public MoneyConvert GetMoneyConverts(Guid moneyConvertId)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => p.IsMain != null && p.IsMain.Value && p.Guid == moneyConvertId)
                .Select(p => new MoneyConvert
                {
                    Guid = p.Guid,
                    BaseAmount = p.BaseAmount,
                    BaseCurrencyId = p.BaseCurrencyId,
                    BaseCurrency = new BaseInfoGeneral
                    {
                        Name = p.BaseCurrency.Name
                    },
                    DestAmount = p.DestAmount,
                    DestCurrencyId = p.DestCurrencyId,
                    DestCurrency = new BaseInfoGeneral
                    {
                        Name = p.DestCurrency.Name
                    }
                }).SingleOrDefault();
        }

        public Guid? GetMoneyConvertIdBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => p.IsMain != null && p.IsMain.Value && p.ClinicSectionId == clinicSectionId && ((p.BaseCurrencyId == baseCurrencyId && p.DestCurrencyId == destCurrencyId) || (p.BaseCurrencyId == destCurrencyId && p.DestCurrencyId == baseCurrencyId)))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .FirstOrDefault()?.Guid;
        }

        public Guid? GetMoneyConvertIdBaseCurrenciesAndAmounts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, decimal? baseAmount, decimal? destAmount)
        {
            return _context.MoneyConverts.AsNoTracking()
                .Include(p => p.BaseCurrency)
                .Include(p => p.DestCurrency)
                .Where(p => (p.IsMain == null || !p.IsMain.Value) && p.ClinicSectionId == clinicSectionId && (
                (p.BaseCurrencyId == baseCurrencyId && p.DestCurrencyId == destCurrencyId && p.BaseAmount == baseAmount && p.DestAmount == destAmount) ||
                (p.BaseCurrencyId == destCurrencyId && p.DestCurrencyId == baseCurrencyId && p.DestAmount == destAmount && p.DestAmount == baseAmount)
                ))
                .OrderByDescending(p => p.Date).ThenByDescending(p => p.Id)
                .FirstOrDefault()?.Guid;
        }

    }
}
