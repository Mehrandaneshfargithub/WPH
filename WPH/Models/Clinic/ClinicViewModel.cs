using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Clinic
{
    public class ClinicViewModel : IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public Nullable<System.Guid> StateId { get; set; }
        public Nullable<System.Guid> CityId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string SystemCode { get; set; }
        public string SystemCodeHolder { get; set; }
        public string licenseNumber { get; set; }
        public Nullable<bool> Active { get; set; }

        //public virtual ICollection<ClinicSectionViewModel> ClinicSections { get; set; }

    }
}