using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class RoomItem
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Item Item { get; set; }
        public virtual Room Room { get; set; }
    }
}
