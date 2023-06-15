using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWebUserFormAccess
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FormAccessId { get; set; }

        public virtual TblWebFormAccess FormAccess { get; set; }
        public virtual TblWebUser User { get; set; }
    }
}
