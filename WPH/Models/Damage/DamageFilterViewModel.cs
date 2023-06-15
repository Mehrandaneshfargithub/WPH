using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Damage
{
    public class DamageFilterViewModel
    {
        public int PeriodId { get; set; }
        public DateTime DateFrom { get; set; }
        public string TxtDateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string TxtDateTo { get; set; }
    }
}
