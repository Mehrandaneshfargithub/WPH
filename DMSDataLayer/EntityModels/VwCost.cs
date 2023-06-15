using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class VwCost
    {
        public int Id { get; set; }
        public int? CostTypeId { get; set; }
        public string CostTypeName { get; set; }
        public DateTime? Data { get; set; }
        public decimal? Price { get; set; }
        public string Desc { get; set; }
    }
}
