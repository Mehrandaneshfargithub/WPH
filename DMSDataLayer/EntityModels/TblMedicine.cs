using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicine
    {
        public TblMedicine()
        {
            TblPurchaseInvoiceDetails = new HashSet<TblPurchaseInvoiceDetail>();
            TblTopMedicines = new HashSet<TblTopMedicine>();
        }

        public int Id { get; set; }
        public string Barcode { get; set; }
        public string ScientificName { get; set; }
        public string JoineryName { get; set; }
        public string Code { get; set; }
        public int? ProducerId { get; set; }
        public int? OrderPoint { get; set; }
        public int? FormId { get; set; }
        public int? GeneralUnitId { get; set; }
        public decimal? GeneralUnitNum { get; set; }
        public int? SecondryUnitId { get; set; }
        public int? SecondryUnitNum { get; set; }
        public int? UnitId { get; set; }
        public int? MaintenanceTypeId { get; set; }
        public string Desc { get; set; }
        public int? Priority { get; set; }
        public string Location { get; set; }
        public decimal? Num { get; set; }
        public decimal? LatestSellingPrice { get; set; }
        public DateTime? LatestExpireDate { get; set; }
        public string Consideration { get; set; }
        public decimal? BaseNum { get; set; }
        public decimal? LatestPurchasePrice { get; set; }
        public DateTime? LatestPurchaseDate { get; set; }
        public string LatestConsideration { get; set; }
        public bool? IsHidden { get; set; }
        public int? ByHand { get; set; }
        public int? Crc { get; set; }
        public bool? Enabled { get; set; }
        public int? TasneefId { get; set; }
        public string StatusC { get; set; }
        public decimal? MnumRemaining { get; set; }
        public decimal? MfreeNumRemaining { get; set; }
        public int? UserId { get; set; }
        public int? MandoobId { get; set; }
        public int? SharikaId { get; set; }
        public decimal? PaakatWeight { get; set; }
        public int? SalesMeasureId { get; set; }

        public virtual ICollection<TblPurchaseInvoiceDetail> TblPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<TblTopMedicine> TblTopMedicines { get; set; }
    }
}
