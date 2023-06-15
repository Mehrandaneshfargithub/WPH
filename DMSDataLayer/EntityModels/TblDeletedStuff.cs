using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblDeletedStuff
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? UserId { get; set; }
        public int? Type { get; set; }
        public string Data { get; set; }

        public virtual TblDeletedType TypeNavigation { get; set; }
        public virtual TblUser User { get; set; }
    }
}
