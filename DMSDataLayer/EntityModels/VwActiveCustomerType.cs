using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwActiveCustomerType
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Priority { get; set; }
    }
}
