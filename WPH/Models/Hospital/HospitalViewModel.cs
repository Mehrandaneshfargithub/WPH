using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.ClinicSection;

namespace WPH.Models.Hospital
{
    public class HospitalViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string NameHolder { get; set; }
       
    }
}
