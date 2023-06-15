using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblUserFormAccess
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FormAccessId { get; set; }
    }
}
