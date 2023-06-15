using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMaktabAcc
    {
        public int? Id { get; set; }
        public bool? IsDollar { get; set; }
        public int? MaktabId { get; set; }
        public string MaktabName { get; set; }
        public DateTime? MyDate { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? RecievedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Desc { get; set; }
        public decimal? TotalMaktabRecieved { get; set; }
        public decimal? TotalMaktabPaid { get; set; }
        public decimal? TotalMaktabExchange { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? TotalRecieved { get; set; }
        public decimal? TotalExchange { get; set; }
        public int? Type { get; set; }
        public decimal? RemainAmount { get; set; }
        public decimal? EndRemain { get; set; }
        public string CreatedUsername { get; set; }
        public string ModifiedUsername { get; set; }
    }
}
