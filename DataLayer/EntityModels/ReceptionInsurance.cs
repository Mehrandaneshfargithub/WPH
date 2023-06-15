using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ReceptionInsurance
    {
        public ReceptionInsurance()
        {
            ReceptionInsuranceReceiveds = new HashSet<ReceptionInsuranceReceived>();
            ReceptionServiceReceiveds = new HashSet<ReceptionServiceReceived>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual User CreatedUser { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual ICollection<ReceptionInsuranceReceived> ReceptionInsuranceReceiveds { get; set; }
        public virtual ICollection<ReceptionServiceReceived> ReceptionServiceReceiveds { get; set; }
    }
}
