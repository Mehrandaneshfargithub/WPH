using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblBankCreatedUsers = new HashSet<TblBank>();
            TblBankModifiedUsers = new HashSet<TblBank>();
            TblCheckinCheckOutUsers = new HashSet<TblCheckinCheckOutUser>();
            TblCombinedInvoiceCreatedUsers = new HashSet<TblCombinedInvoice>();
            TblCombinedInvoiceModifiedUsers = new HashSet<TblCombinedInvoice>();
            TblCostCreatedUsers = new HashSet<TblCost>();
            TblCostModifiedUsers = new HashSet<TblCost>();
            TblDamagedCreatedUsers = new HashSet<TblDamaged>();
            TblDamagedModifiedUsers = new HashSet<TblDamaged>();
            TblDamagedUsers = new HashSet<TblDamaged>();
            TblDeletedStuffs = new HashSet<TblDeletedStuff>();
            TblExchangeInvoiceCreatedUsers = new HashSet<TblExchangeInvoice>();
            TblExchangeInvoiceModifiedUsers = new HashSet<TblExchangeInvoice>();
            TblMaktabPayCreatedUsers = new HashSet<TblMaktabPay>();
            TblMaktabPayModifiedUsers = new HashSet<TblMaktabPay>();
            TblPayCreatedUsers = new HashSet<TblPay>();
            TblPayModifiedUsers = new HashSet<TblPay>();
            TblPrefactorCreateUsers = new HashSet<TblPrefactor>();
            TblPrefactorDetails = new HashSet<TblPrefactorDetail>();
            TblPrefactorModifiedUsers = new HashSet<TblPrefactor>();
            TblPurchasOrderDetails = new HashSet<TblPurchasOrderDetail>();
            TblPurchaseInvoiceCreatedUsers = new HashSet<TblPurchaseInvoice>();
            TblPurchaseInvoiceDetails = new HashSet<TblPurchaseInvoiceDetail>();
            TblPurchaseInvoiceModifiedUsers = new HashSet<TblPurchaseInvoice>();
            TblPurchaseOrderCreateUsers = new HashSet<TblPurchaseOrder>();
            TblPurchaseOrderModifiedUsers = new HashSet<TblPurchaseOrder>();
            TblRecieverCreatedUsers = new HashSet<TblReciever>();
            TblRecieverModifiedUsers = new HashSet<TblReciever>();
            TblReturnPurchaseInvoices = new HashSet<TblReturnPurchaseInvoice>();
            TblReturnSaleInvoices = new HashSet<TblReturnSaleInvoice>();
            TblSaleInvoiceCreatedUsers = new HashSet<TblSaleInvoice>();
            TblSaleInvoiceDetails = new HashSet<TblSaleInvoiceDetail>();
            TblSaleInvoiceModifiedUsers = new HashSet<TblSaleInvoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Pass3 { get; set; }
        public string Pass4 { get; set; }
        public string Mobile { get; set; }
        public int? ThemeId { get; set; }
        public int? FontId { get; set; }

        public virtual TblFont Font { get; set; }
        public virtual TblTheme Theme { get; set; }
        public virtual ICollection<TblBank> TblBankCreatedUsers { get; set; }
        public virtual ICollection<TblBank> TblBankModifiedUsers { get; set; }
        public virtual ICollection<TblCheckinCheckOutUser> TblCheckinCheckOutUsers { get; set; }
        public virtual ICollection<TblCombinedInvoice> TblCombinedInvoiceCreatedUsers { get; set; }
        public virtual ICollection<TblCombinedInvoice> TblCombinedInvoiceModifiedUsers { get; set; }
        public virtual ICollection<TblCost> TblCostCreatedUsers { get; set; }
        public virtual ICollection<TblCost> TblCostModifiedUsers { get; set; }
        public virtual ICollection<TblDamaged> TblDamagedCreatedUsers { get; set; }
        public virtual ICollection<TblDamaged> TblDamagedModifiedUsers { get; set; }
        public virtual ICollection<TblDamaged> TblDamagedUsers { get; set; }
        public virtual ICollection<TblDeletedStuff> TblDeletedStuffs { get; set; }
        public virtual ICollection<TblExchangeInvoice> TblExchangeInvoiceCreatedUsers { get; set; }
        public virtual ICollection<TblExchangeInvoice> TblExchangeInvoiceModifiedUsers { get; set; }
        public virtual ICollection<TblMaktabPay> TblMaktabPayCreatedUsers { get; set; }
        public virtual ICollection<TblMaktabPay> TblMaktabPayModifiedUsers { get; set; }
        public virtual ICollection<TblPay> TblPayCreatedUsers { get; set; }
        public virtual ICollection<TblPay> TblPayModifiedUsers { get; set; }
        public virtual ICollection<TblPrefactor> TblPrefactorCreateUsers { get; set; }
        public virtual ICollection<TblPrefactorDetail> TblPrefactorDetails { get; set; }
        public virtual ICollection<TblPrefactor> TblPrefactorModifiedUsers { get; set; }
        public virtual ICollection<TblPurchasOrderDetail> TblPurchasOrderDetails { get; set; }
        public virtual ICollection<TblPurchaseInvoice> TblPurchaseInvoiceCreatedUsers { get; set; }
        public virtual ICollection<TblPurchaseInvoiceDetail> TblPurchaseInvoiceDetails { get; set; }
        public virtual ICollection<TblPurchaseInvoice> TblPurchaseInvoiceModifiedUsers { get; set; }
        public virtual ICollection<TblPurchaseOrder> TblPurchaseOrderCreateUsers { get; set; }
        public virtual ICollection<TblPurchaseOrder> TblPurchaseOrderModifiedUsers { get; set; }
        public virtual ICollection<TblReciever> TblRecieverCreatedUsers { get; set; }
        public virtual ICollection<TblReciever> TblRecieverModifiedUsers { get; set; }
        public virtual ICollection<TblReturnPurchaseInvoice> TblReturnPurchaseInvoices { get; set; }
        public virtual ICollection<TblReturnSaleInvoice> TblReturnSaleInvoices { get; set; }
        public virtual ICollection<TblSaleInvoice> TblSaleInvoiceCreatedUsers { get; set; }
        public virtual ICollection<TblSaleInvoiceDetail> TblSaleInvoiceDetails { get; set; }
        public virtual ICollection<TblSaleInvoice> TblSaleInvoiceModifiedUsers { get; set; }
    }
}
