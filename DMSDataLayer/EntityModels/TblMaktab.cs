using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblMaktab
    {
        public TblMaktab()
        {
            TblExchangeInvoices = new HashSet<TblExchangeInvoice>();
            TblMaktabPays = new HashSet<TblMaktabPay>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Mob1 { get; set; }
        public string Mob2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Consideration { get; set; }

        public virtual ICollection<TblExchangeInvoice> TblExchangeInvoices { get; set; }
        public virtual ICollection<TblMaktabPay> TblMaktabPays { get; set; }
    }
}
