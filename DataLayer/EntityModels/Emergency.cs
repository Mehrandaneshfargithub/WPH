using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Emergency
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int? ArrivalId { get; set; }
        public int? CriticallyId { get; set; }

        public virtual BaseInfoGeneral Arrival { get; set; }
        public virtual BaseInfoGeneral Critically { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
