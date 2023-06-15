using System;

namespace DataLayer.EntityModels
{
    public class SupplierAccountModel
    {
        public Guid Guid { get; set; }
        public string RecordType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }
        public string Description { get; set; }
        public string PayAmount { get; set; }
        public string GetAmount { get; set; }
        public string PayStatus { get; set; }
        public string PayInvoiceNum { get; set; }
        public string RetunInvoiceNum { get; set; }
        public string MainInvoicePay { get; set; }
    }
}
