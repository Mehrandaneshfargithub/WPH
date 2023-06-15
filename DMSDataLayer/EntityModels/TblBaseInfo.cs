using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblBaseInfo
    {
        public TblBaseInfo()
        {
            TblCustomers = new HashSet<TblCustomer>();
            TblDamageds = new HashSet<TblDamaged>();
            TblReturnPurchaseInvoices = new HashSet<TblReturnPurchaseInvoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public int? TypeId { get; set; }
        public string Description1 { get; set; }

        public virtual TblBaseInfoType Type { get; set; }
        public virtual ICollection<TblCustomer> TblCustomers { get; set; }
        public virtual ICollection<TblDamaged> TblDamageds { get; set; }
        public virtual ICollection<TblReturnPurchaseInvoice> TblReturnPurchaseInvoices { get; set; }
    }
}
