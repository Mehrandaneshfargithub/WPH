using System.Collections.Generic;
using WPH.Models.SupplierAccount;

namespace WPH.Models.CustomerAccount
{
    public class CustomerAccountReportResultViewModel
    {
        public string CustomerName { get; set; }

        public IEnumerable<CustomerAccountReportViewModel> AllSale { get; set; }
        public IEnumerable<CustomerAccountReportDetailViewModel> AllDetailSale { get; set; }
        public IEnumerable<SupplierAccountReportTotalViewModel> Results { get; set; }
    }
}
