using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.EntityModels
{
    public class FN_MedicineNumModel
    {
        public int? Id { get; set; }
        public string JoineryName { get; set; }
        public string ProducerName { get; set; }
        public string MedicineFormName { get; set; }
        public string Barcode { get; set; }
        public decimal? Num { get; set; }
        public decimal? Price { get; set; }
    }
}
