using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblFont
    {
        public TblFont()
        {
            TblUsers = new HashSet<TblUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
