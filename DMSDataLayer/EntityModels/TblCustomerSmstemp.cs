using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCustomerSmstemp
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string CustomersType { get; set; }
        public int? CustomersTypeId { get; set; }
        public string Mob1 { get; set; }
        public string Mob2 { get; set; }
        public decimal? RemaindAccount { get; set; }
        public decimal? RemainAccountDate { get; set; }
    }
}
