using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Service
{
    public class ServiceReportViewModel
    {
        public string ServiceName { get; set; }
        public string Price => Temp_Price.GetValueOrDefault(0).ToString("N0");
        public string TypeName { get; set; }
        public decimal? Temp_Price { get; set; }
    }
}
