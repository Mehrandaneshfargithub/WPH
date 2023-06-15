using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCheckinCheckOutUser
    {
        public int Id { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? Checkout { get; set; }
        public int UserId { get; set; }

        public virtual TblUser User { get; set; }
    }
}
