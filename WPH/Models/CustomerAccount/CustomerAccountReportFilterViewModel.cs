using System;

namespace WPH.Models.CustomerAccount
{
    public class CustomerAccountReportFilterViewModel
    {
        public Guid CustomerId { get; set; }
        public int FilterId { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public DateTime FromDate { get; set; }
        public string TxtFromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TxtToDate { get; set; }
        public bool Detail { get; set; }
    }
}
