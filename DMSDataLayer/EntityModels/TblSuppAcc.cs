using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSuppAcc
    {
        public int? Id { get; set; }
        public int? CustomersId { get; set; }
        public string CustomersName { get; set; }
        public string RecieveDate { get; set; }
        public string SaleInvoiceNum { get; set; }
        public decimal? RecievedAmount { get; set; }
        public decimal? PaidAmount { get; set; }
        public string Desc { get; set; }
        public decimal? TotalCustomerRecieved { get; set; }
        public decimal? TotalCustomerPaid { get; set; }
        public decimal? TotalCustomerSale { get; set; }
        public decimal? TotalCustomerReturnSale { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? TotalRecieved { get; set; }
        public decimal? TotalSale { get; set; }
        public decimal? TotalReturnSale { get; set; }
        public bool? IsRecieved { get; set; }
        public decimal? RemainAmount { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
