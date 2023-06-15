using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblResetInvoiceNum
    {
        public int Id { get; set; }
        public string InvoiceType { get; set; }
        public DateTime? ResetFromDate { get; set; }
        public DateTime? ResetToDate { get; set; }
        public string PreInvoiceNum { get; set; }
    }
}
