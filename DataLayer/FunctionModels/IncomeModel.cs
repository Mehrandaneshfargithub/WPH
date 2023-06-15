using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class IncomeModel
    {
        public DateTime Date { get; set; }
        public decimal SalePrice { get; set; }
        public string CurrencyName { get; set; }
    }
}
