using System;
using System.Collections.Generic;
using System.Globalization;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;

namespace WPH.Models.CustomDataModels.MoneyConvert
{
    public class MoneyConvertViewModel : IndexViewModel
    {
        CultureInfo cultures = new CultureInfo("en-US");

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int BaseCurrencyId { get; set; }
        public int DestCurrencyId { get; set; }
        public DateTime? Date { get; set; }
        public string DateTxt { get; set; }
        public string BaseCurrencyName { get; set; }
        public string DestCurrencyName { get; set; }
        public decimal? BaseAmount { get; set; }
        public string BaseAmountTxt => BaseAmount.GetValueOrDefault(1).ToString("0.##", cultures);
        public decimal? DestAmount { get; set; }
        public string DestAmountTxt => DestAmount.GetValueOrDefault(1).ToString("0.##", cultures);
        public decimal? Amount { get; set; }
        public bool? IsMain { get; set; }
        public string ShowMoneyConvert => $"{BaseAmount.GetValueOrDefault(0).ToString("#,##", cultures)} {BaseCurrencyName} = {DestAmount.GetValueOrDefault(0).ToString("#,##", cultures)} {DestCurrencyName}";

        public virtual BaseInfoGeneralViewModel BaseCurrency { get; set; }
        public virtual BaseInfoGeneralViewModel DestCurrency { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public IEnumerable<PeriodsViewModel> periods { get; set; }
    }
}