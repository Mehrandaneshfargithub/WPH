using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.RoomItem;

namespace WPH.Models.ReceptionRoomBed
{
    public class ReceptionRoomBedViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? EntranceDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ModifiedUserId { get; set; }
        public Guid? RoomBedId { get; set; }
        public string RoomBedName { get; set; }
        public string RoomName { get; set; }
        public string PatientName { get; set; }
    }
}
