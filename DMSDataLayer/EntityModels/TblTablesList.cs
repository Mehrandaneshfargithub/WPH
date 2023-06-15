using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblTablesList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Tracked { get; set; }
    }
}
