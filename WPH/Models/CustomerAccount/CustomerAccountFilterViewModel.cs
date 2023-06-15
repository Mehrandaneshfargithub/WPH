using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.CustomerAccount
{
    public class CustomerAccountFilterViewModel
    {
        public Guid? CustomerId { get; set; }
        public int? CurrencyId { get; set; }
        public int CustomerFilter { get; set; }
        public int Year { get; set; }
        public DateTime DateFrom { get; set; }
        public string DateFromTxt { get; set; }
        public DateTime DateTo { get; set; }
        public string DateToTxt { get; set; }
    }
}
