using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSummary
    {
        public int Id { get; set; }
        public DateTime SummDate { get; set; }
        public decimal SaleAmount { get; set; }
        public decimal ReturnSaleAmount { get; set; }
        public decimal SaleProfit { get; set; }
        public decimal RebhDamagedAmount { get; set; }
        public decimal RebhProfitAmount { get; set; }
        public decimal CostAmount { get; set; }
    }
}
