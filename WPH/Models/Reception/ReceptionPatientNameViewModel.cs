using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Reception
{
    public class ReceptionPatientNameViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string NamePhone => $"{Name}|{PhoneNumber}";
    }
}
