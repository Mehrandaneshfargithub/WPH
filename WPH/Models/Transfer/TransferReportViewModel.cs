using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Transfer
{
    public class TransferReportViewModel
    {
        public DateTime FromDate { get; set; }
        public string TxtFromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string TxtToDate { get; set; }
        public string ReceiverName { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? SourceClinicSectionId { get; set; }
        public Guid? DestinationClinicSectionId { get; set; }

        public List<Guid> AllClinicSectionGuids { get; set; }
    }
}
