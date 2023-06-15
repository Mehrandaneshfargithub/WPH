using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoice
    {
        [NotMapped]
        public string TotalDiscount { get; set; }
        [NotMapped]
        public bool Status { get; set; }
    }
}
