using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicineImage
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public int? Crc { get; set; }
    }
}
