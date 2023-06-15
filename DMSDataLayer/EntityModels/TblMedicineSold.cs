using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicineSold
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public int SoldCount { get; set; }
        public DateTime SoldDate { get; set; }
    }
}
