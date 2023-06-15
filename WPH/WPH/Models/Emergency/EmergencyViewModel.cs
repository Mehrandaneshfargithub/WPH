using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Reception;

namespace WPH.Models.Emergency
{
    public class EmergencyViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int? ArrivalId { get; set; }
        public int? CriticallyId { get; set; }

        public virtual BaseInfoGeneralViewModel Arrival { get; set; }
        public virtual BaseInfoGeneralViewModel Critically { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
    }
}
