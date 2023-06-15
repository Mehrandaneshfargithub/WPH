using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblChangeTrackingSettingClient
    {
        public int Id { get; set; }
        public string TablePrefix { get; set; }
        public string ClientName { get; set; }
        public int? ClientNumber { get; set; }
    }
}
