using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IMoneyConvertRepository : IRepository<MoneyConvert>
    {
        IEnumerable<MoneyConvert> GetAllMoneyConvertByDate(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo);
        void RemoveMoneyConvert(Guid moneyConvertId);
        decimal? GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        MoneyConvert GetMoneyConvertBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        Guid? GetMoneyConvertIdBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        IEnumerable<MoneyConvert> GetLatestMoneyConverts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        IEnumerable<MoneyConvert> GetLatestMoneyConvertsWithIsMain(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        MoneyConvert GetMoneyConverts(Guid moneyConvertId);
        Guid? GetMoneyConvertIdBaseCurrenciesAndAmounts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, decimal? baseAmount, decimal? destAmount);
    }
}
