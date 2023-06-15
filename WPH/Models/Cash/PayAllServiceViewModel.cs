using System;
using WPH.Models.ReceptionInsurance;

namespace WPH.Models.Cash
{
    public class PayAllServiceViewModel
    {
        public Guid ReceptionId { get; set; }
        public Guid UserId { get; set; }
        public Guid CreatedUserId { get; set; }
        public string PayerName { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public decimal Insurance { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? RecievedDate { get; set; }
        public string RecievedDateTxt { get; set; }
        public virtual ReceptionInsuranceViewModel ReceptionInsurance { get; set; }

    }
}
