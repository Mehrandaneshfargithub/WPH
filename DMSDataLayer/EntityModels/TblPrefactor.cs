using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPrefactor
    {
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CustomerId { get; set; }
        public string Description { get; set; }
        public int? CreateUserId { get; set; }
        public bool? IsDollar { get; set; }
        public int? ModifiedUserId { get; set; }
        public decimal? TotalPrice { get; set; }
        public string FactorType { get; set; }
        public string Description2 { get; set; }

        public virtual TblUser CreateUser { get; set; }
        public virtual TblCustomer Customer { get; set; }
        public virtual TblUser ModifiedUser { get; set; }
    }
}
