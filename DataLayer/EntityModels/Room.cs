using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Room
    {
        public Room()
        {
            Children = new HashSet<Child>();
            RoomBeds = new HashSet<RoomBed>();
            RoomItems = new HashSet<RoomItem>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxCapacity { get; set; }
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral Status { get; set; }
        public virtual BaseInfoGeneral Type { get; set; }
        public virtual ICollection<Child> Children { get; set; }
        public virtual ICollection<RoomBed> RoomBeds { get; set; }
        public virtual ICollection<RoomItem> RoomItems { get; set; }
    }
}
