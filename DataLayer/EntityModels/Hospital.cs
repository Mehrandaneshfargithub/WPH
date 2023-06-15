using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Hospital
    {
        public Hospital()
        {
            Ambulances = new HashSet<Ambulance>();
            ReceptionAmbulanceFromHospitals = new HashSet<ReceptionAmbulance>();
            ReceptionAmbulanceToHospitals = new HashSet<ReceptionAmbulance>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Ambulance> Ambulances { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulanceFromHospitals { get; set; }
        public virtual ICollection<ReceptionAmbulance> ReceptionAmbulanceToHospitals { get; set; }
    }
}
