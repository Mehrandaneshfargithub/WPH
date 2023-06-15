using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;

namespace WPH.Models.CustomDataModels.Cost
{
    public class CostViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public Guid? CostTypeId { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? Price { get; set; }
        public string PriceTxt { get; set; }
        public string Explanation { get; set; }
        public string InvoiceNum { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? OriginalClinicSectionId { get; set; }
        public Guid? UserId { get; set; }
        public int? CurrencyId { get; set; }
        public string CostTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string CostDateDay { get; set; }
        public string CostDateMonth { get; set; }
        public string CostDateYear { get; set; }
        public Guid? PurchaseInvoiceId { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public BaseInfoViewModel CostType { get; set; }
        public BaseInfoGeneralViewModel Currency { get; set; }
        public List<ClinicSectionSettingValueViewModel> AllDecimalAmount { get; set; }

    }
}