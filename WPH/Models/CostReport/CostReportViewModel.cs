using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.CostReport
{
    public class CostReportViewModel
    {
        public Nullable<System.Guid> CostTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public string ClinicSectionName { get; set; }
        public string CostTypeName { get; set; }
        public string Total { get; set; }


        public List<CostReportViewModel> AllCost { get; set; }
        public List<CostReportViewModel> AllClinicSectionTypeCostTotal { get; set; }
        public List<CostReportViewModel> AllSectionsTotal { get; set; }
        public List<CostReportViewModel> AllTypeTotal { get; set; }
    }
}