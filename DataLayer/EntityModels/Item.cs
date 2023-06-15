using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Item
    {
        public Item()
        {
            RoomItems = new HashSet<RoomItem>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ItemTypeId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? SectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfo ItemType { get; set; }
        public virtual BaseInfo Section { get; set; }
        public virtual ICollection<RoomItem> RoomItems { get; set; }
    }
}
