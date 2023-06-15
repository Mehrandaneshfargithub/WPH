using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblZoneMandoob
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int MandoobId { get; set; }
    }
}
