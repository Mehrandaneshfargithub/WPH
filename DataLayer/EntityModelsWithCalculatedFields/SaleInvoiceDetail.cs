using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoiceDetail
    {
        [NotMapped]
        public string ChildrenGuids { get; set; }
        [NotMapped]
        public int ChildrenCount { get; set; }
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
