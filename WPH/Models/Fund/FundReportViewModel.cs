using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.PurchaseInvoice;

namespace WPH.Models.Fund
{
    public class FundReportViewModel
    {
        public List<FundViewModel> AllFund { get; set; }
        public List<FundViewModel> AllSectionsTotal { get; set; }
        public List<FundViewModel> AllAnalysis { get; set; }
        public List<FundViewModel> DoctorFund { get; set; }
        public List<PurchaseInvoiceReportViewModel> PurchaseFund { get; set; }
        public List<PurchaseInvoiceReportViewModel> PurchaseFundDetail { get; set; }
        public List<PurchaseInvoiceReportViewModel> TotalCurrency { get; set; }

        public string Total { get; set; }
    }
}
