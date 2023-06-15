using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Chart
{
    public class IncomeViewModel
    {
        public string CurrencyName { get; set; }
        public string[] Date { get; set; }
        public decimal[] Value { get; set; }
    }
}
