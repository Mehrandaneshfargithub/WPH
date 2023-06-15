using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class UserSubSystemAccess
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int SubSystemAccessId { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual SubSystemAccess SubSystemAccess { get; set; }
        public virtual User User { get; set; }
    }
}
