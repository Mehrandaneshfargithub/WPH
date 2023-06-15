using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblActiveSupplierType
    {
        public int Id { get; set; }
        public int SupplierTypeId { get; set; }
        public bool Active { get; set; }
    }
}
