using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Transfer
{
    public class TransferFilterViewModel
    {
        public int PeriodId { get; set; }
        public DateTime DateFrom { get; set; }
        public string TxtDateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string TxtDateTo { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? TypeId { get; set; }
    }
}
