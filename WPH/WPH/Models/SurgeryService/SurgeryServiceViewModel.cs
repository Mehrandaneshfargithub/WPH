using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Service;
using WPH.Models.Surgery;

namespace WPH.Models.SurgeryService
{
    public class SurgeryServiceViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? SurgeryId { get; set; }

        public virtual ServiceViewModel Service { get; set; }
        public virtual SurgeryViewModel Surgery { get; set; }
    }
}
