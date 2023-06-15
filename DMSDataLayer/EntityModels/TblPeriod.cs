using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPeriod
    {
        public int Id { get; set; }
        public string Period { get; set; }
        public bool? Active { get; set; }
        public bool? Closed { get; set; }
        public bool? Selected { get; set; }
    }
}
