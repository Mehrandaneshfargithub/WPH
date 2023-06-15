using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Ambulance
{
    public class AmbulanceViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? HospitalId { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string HospitalName { get; set; }
        public string Code { get; set; }
        public bool? Active { get; set; }

    }
}
