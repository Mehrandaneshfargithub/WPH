using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblActiveUserFormType
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FormId { get; set; }
        public bool Active { get; set; }
    }
}
