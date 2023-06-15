using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWebFormAccess
    {
        public TblWebFormAccess()
        {
            TblWebUserFormAccesses = new HashSet<TblWebUserFormAccess>();
        }

        public int Id { get; set; }
        public int FormId { get; set; }
        public int AccessId { get; set; }

        public virtual TblWebFormtblAccess Access { get; set; }
        public virtual TblWebFormtblAccess Form { get; set; }
        public virtual ICollection<TblWebUserFormAccess> TblWebUserFormAccesses { get; set; }
    }
}
