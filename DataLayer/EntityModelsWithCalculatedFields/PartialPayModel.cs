using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class PartialPayModel
    {
        public Guid Guid { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Description { get; set; }
        public string MainInvoiceNum { get; set; }
        public string TotalPrice { get; set; }
        public string PayIds { get; set; }
    }
}
