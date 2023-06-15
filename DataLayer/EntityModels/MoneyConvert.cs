using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class MoneyConvert
    {
        public MoneyConvert()
        {
            PurchaseInvoiceDetailSalePrices = new HashSet<PurchaseInvoiceDetailSalePrice>();
            SaleInvoiceDetails = new HashSet<SaleInvoiceDetail>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int BaseCurrencyId { get; set; }
        public int DestCurrencyId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? DestAmount { get; set; }
        public bool? IsMain { get; set; }

        public virtual BaseInfoGeneral BaseCurrency { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral DestCurrency { get; set; }
        public virtual ICollection<PurchaseInvoiceDetailSalePrice> PurchaseInvoiceDetailSalePrices { get; set; }
        public virtual ICollection<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
    }
}
