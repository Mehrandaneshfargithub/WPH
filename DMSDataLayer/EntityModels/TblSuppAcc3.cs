using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblSuppAcc3
    {
        public int? Id { get; set; }
        public int? CustomersId { get; set; }
        public string CustomersName { get; set; }
        public string QabzNum { get; set; }
        public DateTime? MyDate { get; set; }
        public string Date { get; set; }
        public string InvoiceNum { get; set; }
        public decimal? RecievedAmount { get; set; }
        public decimal? RecievedAmountSecond { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? PaidAmountSecond { get; set; }
        public string Desc { get; set; }
        public decimal? TotalCustomerRecieved { get; set; }
        public decimal? TotalCustomerRecievedSecond { get; set; }
        public decimal? TotalCustomerPaid { get; set; }
        public decimal? TotalCustomerPaidSecond { get; set; }
        public decimal? TotalCustomerSale { get; set; }
        public decimal? TotalCustomerSaleSecond { get; set; }
        public decimal? TotalCustomerReturnSale { get; set; }
        public decimal? TotalCustomerReturnSaleSecond { get; set; }
        public int? Type { get; set; }
        public decimal? RemainAmount { get; set; }
        public decimal? RemainAmountSecond { get; set; }
        public decimal? EndRemain { get; set; }
        public decimal? EndRemainSecond { get; set; }
        public int? PaidFactor { get; set; }
        public string CreatedUsername { get; set; }
        public string ModifiedUsername { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
