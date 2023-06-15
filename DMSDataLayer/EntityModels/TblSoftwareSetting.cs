using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSoftwareSetting
    {
        public int Id { get; set; }
        public string DateFormat { get; set; }
        public int? PageSize { get; set; }
        public int? NewSaleCount { get; set; }
        public int? NewPurchaseCount { get; set; }
        public int? CashCustomerId { get; set; }
        public int? MayaSharikiSupplierTypeId { get; set; }
        public int? JardSupplierTypeId { get; set; }
        public decimal? EftetahSandoq { get; set; }
        public DateTime? EftetahSandoqDate { get; set; }
        public bool? LatestPrice { get; set; }
        public bool? PurchaseDollarBased { get; set; }
        public bool? SaleDollarBased { get; set; }
        public bool? BujNumberExpireDateControl { get; set; }
        public int? NewQabzCount { get; set; }
        public bool? CustomerDiscount { get; set; }
        public int? AutoSaveSeconds { get; set; }
        public string ProjectType { get; set; }
        public string Company { get; set; }
        public string CityName { get; set; }
        public int? SaleJoineryNameColSize { get; set; }
        public int? CombinedCustomerId { get; set; }
        public int? CombinedSupplierId { get; set; }
        public bool? AdvancedSecurityNewVersion { get; set; }
        public bool? ImpactionOfDollarDinarPriceChangingOnSellingPrice { get; set; }
        public bool? Esale { get; set; }
        public int? WebStoreCheckTime { get; set; }
        public int? ConcurrentlyDisplay { get; set; }
        public int? AlarmShowingTime { get; set; }
        public int? JardCustomerId { get; set; }
        public DateTime? LatestJardDate { get; set; }
        public int? CashSupplierId { get; set; }
        public int? DinarPoints { get; set; }
        public bool? ShowCustomerDiscount { get; set; }
        public string Exd { get; set; }
        public string Exds { get; set; }
        public int? RemoteCustomerId { get; set; }
        public int? MandoobTypeId { get; set; }
        public int? DefaultWarehouseId { get; set; }
        public int? FactorCostTypeId { get; set; }
    }
}
