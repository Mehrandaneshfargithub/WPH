using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicine
    {
        public string ProducerName { get; set; }
        public string MedicineFormName { get; set; }
        //public virtual FN_MedicineNumModel MdNum { get; set; }
    }
}
