using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblAutoSaleClinic
    {
        public int Id { get; set; }
        public string HostDataSource { get; set; }
        public string HostDbname { get; set; }
        public string HostUserName { get; set; }
        public string HostPass { get; set; }
        public Guid? ClinicId { get; set; }
    }
}
