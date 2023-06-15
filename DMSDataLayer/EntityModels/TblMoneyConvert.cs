using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMoneyConvert
    {
        public int Id { get; set; }
        public DateTime ConvertDate { get; set; }
        public decimal Dollar { get; set; }
        public decimal Dinar { get; set; }
    }
}
