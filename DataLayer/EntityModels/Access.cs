using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Access
    {
        public Access()
        {
            SubSystemAccesses = new HashSet<SubSystemAccess>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubSystemAccess> SubSystemAccesses { get; set; }
    }
}
