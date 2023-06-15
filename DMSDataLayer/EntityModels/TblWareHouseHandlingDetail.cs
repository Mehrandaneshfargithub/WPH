using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWareHouseHandlingDetail
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalePrice { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string BujNumber { get; set; }
        public decimal? Num { get; set; }
        public decimal? FreeNum { get; set; }
        public int? MasterId { get; set; }
        public int? UserId { get; set; }
        public decimal? Packet { get; set; }
        public decimal? NumInPacket { get; set; }
        public string Consideration { get; set; }
        public decimal? Discount { get; set; }
    }
}
