using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ReceptionInsuranceReceived
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionInsuranceId { get; set; }
        public string PayerName { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? AmountStatus { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual ReceptionInsurance ReceptionInsurance { get; set; }
    }
}
