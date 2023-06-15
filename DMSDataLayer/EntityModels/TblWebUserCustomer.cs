using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWebUserCustomer
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }

        public virtual TblCustomer Customer { get; set; }
        public virtual TblWebUser User { get; set; }
    }
}
