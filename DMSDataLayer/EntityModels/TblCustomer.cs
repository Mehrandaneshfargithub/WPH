using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblCustomer
    {
        public TblCustomer()
        {
            TblPrefactors = new HashSet<TblPrefactor>();
            TblRecievers = new HashSet<TblReciever>();
            TblReturnSaleInvoices = new HashSet<TblReturnSaleInvoice>();
            TblSaleInvoices = new HashSet<TblSaleInvoice>();
            TblWebUserCustomers = new HashSet<TblWebUserCustomer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CustomersTypeId { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Mob1 { get; set; }
        public string Mob2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public int? Priority { get; set; }
        public int? MaxCredit { get; set; }
        public decimal? Caccount { get; set; }
        public decimal? Discount { get; set; }
        public int? Month { get; set; }
        public int? BlockStatus { get; set; }
        public string Code { get; set; }
        public bool? AutoPurchase { get; set; }
        public int? CityId { get; set; }
        public bool? SaleBasedOnDollar { get; set; }
        public decimal? CaccountSecond { get; set; }
        public int? DaysWaiteForPayMoney { get; set; }
        public int? MandoobId { get; set; }
        public string StatusC { get; set; }
        public bool? RetailOrWholesale { get; set; }
        public decimal? CustomerProfit { get; set; }
        public bool? OrdinaryProfit { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseeMobile { get; set; }
        public string Sponsor { get; set; }
        public string ExtraPhone { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public string Desc { get; set; }
        public int? ZoneId { get; set; }

        public virtual TblBaseInfo CustomersType { get; set; }
        public virtual ICollection<TblPrefactor> TblPrefactors { get; set; }
        public virtual ICollection<TblReciever> TblRecievers { get; set; }
        public virtual ICollection<TblReturnSaleInvoice> TblReturnSaleInvoices { get; set; }
        public virtual ICollection<TblSaleInvoice> TblSaleInvoices { get; set; }
        public virtual ICollection<TblWebUserCustomer> TblWebUserCustomers { get; set; }
    }
}
