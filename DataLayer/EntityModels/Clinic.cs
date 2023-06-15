using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Clinic
    {
        public Clinic()
        {
            ClinicSections = new HashSet<ClinicSection>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? StateId { get; set; }
        public Guid? CityId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string SystemCode { get; set; }
        public string LicenseNumber { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<ClinicSection> ClinicSections { get; set; }
    }
}
