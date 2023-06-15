using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblActiveCustomerType
    {
        public int Id { get; set; }
        public int CustomerTypeId { get; set; }
        public bool Active { get; set; }
    }
}
