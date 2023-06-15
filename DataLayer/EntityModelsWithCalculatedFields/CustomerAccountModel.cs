using System;

namespace DataLayer.EntityModels
{
    public class CustomerAccountModel
    {
        public Guid Guid { get; set; }
        public string RecordType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public string Description { get; set; }
        public string ReceiveAmount { get; set; }
        public string GetAmount { get; set; }
        public string ReceiveStatus { get; set; }
        public string ReceiveInvoiceNum { get; set; }
        public string RetunInvoiceNum { get; set; }

    }
}
