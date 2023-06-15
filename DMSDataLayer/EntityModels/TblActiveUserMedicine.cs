using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblActiveUserMedicine
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool ShowHiddenMedicine { get; set; }
        public bool ShowHiddenMedicineOptions { get; set; }
    }
}
