using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMedicinesEquivalent
    {
        public int Id { get; set; }
        public int? HostMedId { get; set; }
        public int? LocalMedId { get; set; }
    }
}
