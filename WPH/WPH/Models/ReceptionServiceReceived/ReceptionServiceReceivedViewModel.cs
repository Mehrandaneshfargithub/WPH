using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ReceptionServiceReceived
{
    public class ReceptionServiceReceivedViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionServiceId { get; set; }
        public string PayerName { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? AmountStatus { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public Guid? InstallmentId { get; set; }
    }
}
