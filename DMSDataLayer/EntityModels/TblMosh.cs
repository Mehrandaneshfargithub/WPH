using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMosh
    {
        public string Mcode { get; set; }
        public string Mname { get; set; }
        public string Mgroup { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Text42 { get; set; }
        public bool Block { get; set; }
    }
}
