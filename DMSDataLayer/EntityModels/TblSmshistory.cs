using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSmshistory
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Mob { get; set; }
        public DateTime? Smsdate { get; set; }
        public string Message { get; set; }
        public int? Smsstatus { get; set; }
    }
}
