using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Pay
{
    public class PayViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime? PayDate { get; set; }
        public string PayDateTxt { get; set; }
        public string Description { get; set; }
        public string PayType { get; set; }
        public string InvoiceNum { get; set; }
        public string MainInvoiceNum { get; set; }

        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public IEnumerable<PayAmountViewModel> PayAmounts { get; set; }
    }
}