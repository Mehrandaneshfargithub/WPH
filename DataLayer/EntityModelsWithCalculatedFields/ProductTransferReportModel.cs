using System;

namespace DataLayer.EntityModels
{
    public class ProductTransferReportModel
    {
        public Guid Guid { get; set; }
        public int InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Name { get; set; }
        public decimal? Num { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Consideration { get; set; }
        public string CurrencyName { get; set; }
    }
}
