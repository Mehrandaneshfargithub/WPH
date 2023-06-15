using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSupplier
    {
        public TblSupplier()
        {
            TblAutoPurchaseSuppliers = new HashSet<TblAutoPurchaseSupplier>();
            TblExchangeDetails = new HashSet<TblExchangeDetail>();
            TblPays = new HashSet<TblPay>();
            TblPurchaseInvoices = new HashSet<TblPurchaseInvoice>();
            TblReturnPurchaseInvoices = new HashSet<TblReturnPurchaseInvoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public string Tel { get; set; }
        public string Mob { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public int? Priority { get; set; }
        public int? SupplierTypeId { get; set; }
        public decimal? Saccount { get; set; }
        public int? CustomerId { get; set; }
        public string Code { get; set; }
        public decimal? SaccountSecond { get; set; }
        public string StatusC { get; set; }
        public string Consideration { get; set; }
        public bool? PurchaseBasedOnDollar { get; set; }
        public decimal? Discount { get; set; }

        public virtual ICollection<TblAutoPurchaseSupplier> TblAutoPurchaseSuppliers { get; set; }
        public virtual ICollection<TblExchangeDetail> TblExchangeDetails { get; set; }
        public virtual ICollection<TblPay> TblPays { get; set; }
        public virtual ICollection<TblPurchaseInvoice> TblPurchaseInvoices { get; set; }
        public virtual ICollection<TblReturnPurchaseInvoice> TblReturnPurchaseInvoices { get; set; }
    }
}
