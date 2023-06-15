using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class SubSystemAccess
    {
        public SubSystemAccess()
        {
            UserSubSystemAccesses = new HashSet<UserSubSystemAccess>();
        }

        public int Id { get; set; }
        public int SubSystemId { get; set; }
        public int AccessId { get; set; }

        public virtual Access Access { get; set; }
        public virtual SubSystem SubSystem { get; set; }
        public virtual ICollection<UserSubSystemAccess> UserSubSystemAccesses { get; set; }
    }
}
