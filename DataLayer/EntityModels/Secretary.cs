using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Secretary
    {
        public Secretary()
        {
            ReserveDetails = new HashSet<ReserveDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int? PostId { get; set; }

        public virtual User Gu { get; set; }
        public virtual BaseInfoGeneral Post { get; set; }
        public virtual ICollection<ReserveDetail> ReserveDetails { get; set; }
    }
}
