using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.CustomDataModels.MoneyConvert;

namespace WPH.MvcMockingServices.Interface
{
    public interface IMoneyConvertMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<MoneyConvertViewModel> GetAllMoneyConvertByDate(Guid clinicSectionId, int periodId, DateTime DateFrom, DateTime DateTo);

        decimal? GetMoneyConvertAmountByBaseCurrencyIdAndDestCurrencyId(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        string AddNewMoneyConvert(MoneyConvertViewModel viewModel);
        MoneyConvertViewModel GetMoneyConvertById(Guid id);
        string UpdateMoneyConvert(MoneyConvertViewModel viewModel);
        OperationStatus RemoveMoneyConvert(Guid MoneyConvertId);
        MoneyConvertViewModel GetMoneyConvertBaseCurrencies(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId);
        MoneyConvertViewModel GetMoneyConvertBaseCurrencyName(Guid clinicSectionId, string baseCurrencyName, string destCurrencyName);
        IEnumerable<MoneyConvertViewModel> GetLatestMoneyConverts(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId);
        IEnumerable<MoneyConvertViewModel> GetLatestMoneyConvertsWithIsMain(Guid clinicSectionId, int baseCurrencyId, int destCurrencyId, Guid? moneyConvertId);
    }
}
