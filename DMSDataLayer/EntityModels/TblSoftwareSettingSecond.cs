using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSoftwareSettingSecond
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Warehouse { get; set; }
        public string PreSaleInvoiceNum { get; set; }
        public bool? SendStatus { get; set; }
        public bool? RecieveStatus { get; set; }
    }
}
