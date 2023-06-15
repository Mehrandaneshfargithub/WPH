using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Transfer
{
    public class TransferReportResultViewModel
    {
        public string Date { get; set; }
        public string ReceiverName { get; set; }
        public string CreatedUserName { get; set; }
        public string SourceClinicSectionName { get; set; }
        public string DestinationClinicSectionName { get; set; }
        public string ProductName { get; set; }
        public string DestinationProduct { get; set; }
        public string Num { get; set; }
    }
}
