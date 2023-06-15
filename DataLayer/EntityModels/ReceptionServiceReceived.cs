using System;

namespace DataLayer.EntityModels
{
    public partial class ReceptionServiceReceived
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionServiceId { get; set; }
        public string PayerName { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? AmountStatus { get; set; }
        public Guid? ReceptionInsuranceId { get; set; }
        public Guid? InstallmentId { get; set; }


        public virtual User CreatedUser { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual ReceptionInsurance ReceptionInsurance { get; set; }
        public virtual ReceptionService ReceptionService { get; set; }
    }
}
