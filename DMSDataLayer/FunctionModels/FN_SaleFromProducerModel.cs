using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.EntityModels
{
    public class FN_SaleFromProducerModel
    {
        public string JoineryName { get; set; }
        public string ProducerName { get; set; }
        public string FormName { get; set; }
        public string CustomerName { get; set; }
        public string Barcode { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? Num { get; set; }
        
    }
}
