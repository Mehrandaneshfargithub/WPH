using System.Collections.Generic;

namespace WPH.Models.SupplierAccount
{
    public class SupplierAccountReportResultViewModel
    {
        public string SupplierName { get; set; }
        //public string PurchaseTotal { get; set; }
        //public string ReturnTotal { get; set; }
        //public string Total { get; set; }
        public IEnumerable<SupplierAccountReportViewModel> AllPurchase { get; set; }
        public IEnumerable<SupplierAccountReportDetailViewModel> AllDetailPurchase { get; set; }
        public IEnumerable<SupplierAccountReportTotalViewModel> Results { get; set; }
    }
}
