using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.SupplierAccount
{
    public class SupplierAccountFilterViewModel
    {
        public Guid? SupplierId { get; set; }
        public int? CurrencyId { get; set; }
        public int SupplierFilter { get; set; }
        public int Year { get; set; }
        public DateTime DateFrom { get; set; }
        public string DateFromTxt { get; set; }
        public DateTime DateTo { get; set; }
        public string DateToTxt { get; set; }
    }
}
