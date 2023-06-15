using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionRoomBed
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public DateTime? EntranceDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid ModifiedUserId { get; set; }
        public Guid? RoomBedId { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual RoomBed RoomBed { get; set; }
    }
}
