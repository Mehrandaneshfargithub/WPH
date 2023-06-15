using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblTopMedicine
    {
        public int Id { get; set; }
        public int? MedId { get; set; }

        public virtual TblMedicine Med { get; set; }
    }
}
