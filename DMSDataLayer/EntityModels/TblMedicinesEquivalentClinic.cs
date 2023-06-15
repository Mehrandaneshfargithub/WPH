using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicinesEquivalentClinic
    {
        public int Id { get; set; }
        public int AutoSaleId { get; set; }
        public Guid? HostMedId { get; set; }
        public int? LocalMedId { get; set; }
    }
}
