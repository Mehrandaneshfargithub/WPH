using System;

namespace WPH.Models.RoomBed
{
    public class PatientRoomBedViewModel
    {
        public Guid OldReceptionValue { get; set; }
        public Guid SetReceptionValue { get; set; }
        public Guid OldRoomBedValue { get; set; }
        public Guid SetRoomBedValue { get; set; }
        public Guid UserId { get; set; }
    }
}
