using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblFormAccess
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int AccessId { get; set; }
    }
}
