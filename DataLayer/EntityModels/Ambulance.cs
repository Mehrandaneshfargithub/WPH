using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Ambulance
    {
        public Ambulance()
        {
            ReceptionAmbulances = new HashSet<ReceptionAmbulance>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? HospitalId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? Active { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulances { get; set; }
    }
}
