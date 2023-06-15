using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicineBarcode
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public int? MedicineId { get; set; }
    }
}
