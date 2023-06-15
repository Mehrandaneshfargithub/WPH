using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Room
{
    public class RoomViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxCapacity { get; set; }
        public int? TypeId { get; set; }
        public int? RoomStatusId { get; set; }
        public Guid? RoomClinicSectionId { get; set; }
        public int EmptyBed { get; set; }

        public string NameHolder { get; set; }
        public string RoomType { get; set; }
        public string RoomStatus { get; set; }
        public string SectionName { get; set; }
        public string SectionRoomName => $"{SectionName}|{Name}";
    }

}
