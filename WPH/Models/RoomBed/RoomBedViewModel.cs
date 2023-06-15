using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels;

namespace WPH.Models.RoomBed
{
    public class RoomBedViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid? RoomId { get; set; }
        public int? BedStatusId { get; set; }
        public string NameHolder { get; set; }
        public string RoomName { get; set; }
        public string BedStatus { get; set; }
        public string PatientName { get; set; }
        public string RoomBedName => $"{RoomName}|{Name}";
        public Guid? ReceptionId { get; set; }
    }
}