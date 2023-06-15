using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class RoomBed
    {
        public RoomBed()
        {
            ReceptionRoomBeds = new HashSet<ReceptionRoomBed>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid? RoomId { get; set; }
        public int? StatusId { get; set; }

        public virtual Room Room { get; set; }
        public virtual BaseInfoGeneral Status { get; set; }
        public virtual ICollection<ReceptionRoomBed> ReceptionRoomBeds { get; set; }
    }
}
