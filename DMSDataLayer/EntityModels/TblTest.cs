using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblTest
    {
        public int Id { get; set; }
        public string FormName { get; set; }
        public string ControlName { get; set; }
        public bool? Result { get; set; }
        public string Discreption { get; set; }
    }
}
