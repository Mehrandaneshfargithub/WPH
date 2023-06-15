using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblDeletedType
    {
        public TblDeletedType()
        {
            TblDeletedStuffs = new HashSet<TblDeletedStuff>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Persian { get; set; }
        public int? Priority { get; set; }

        public virtual ICollection<TblDeletedStuff> TblDeletedStuffs { get; set; }
    }
}
