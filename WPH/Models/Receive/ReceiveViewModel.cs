using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Receive
{
    public class ReceiveViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string ReceiveDateTxt { get; set; }
        public string Description { get; set; }
        public string ReceiveType { get; set; }
        public string InvoiceNum { get; set; }

        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? SaleInvoiceId { get; set; }

        public IEnumerable<ReceiveAmountViewModel> ReceiveAmounts { get; set; }
    }
}