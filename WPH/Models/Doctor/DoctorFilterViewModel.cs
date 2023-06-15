using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Doctor
{
    public class DoctorFilterViewModel
    {
        public Guid Guid { get; set; }
        public string UserName { get; set; }
        public string SpecialityName { get; set; }
        public string NameAndSpeciallity => $"{UserName ?? ""} | {SpecialityName ?? ""}";
    }
}
