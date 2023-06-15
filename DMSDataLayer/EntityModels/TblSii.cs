using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSii
    {
        public int Id { get; set; }
        public string Sii { get; set; }
        public int LoginCount { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
