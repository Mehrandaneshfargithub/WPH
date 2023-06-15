using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblTheme
    {
        public TblTheme()
        {
            TblUsers = new HashSet<TblUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TblUser> TblUsers { get; set; }
    }
}
