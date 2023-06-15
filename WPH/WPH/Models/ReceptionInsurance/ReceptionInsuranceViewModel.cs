using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.Models.ReceptionServiceReceived;

namespace WPH.Models.ReceptionInsurance
{
    public class ReceptionInsuranceViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ReceptionPatientUserName { get; set; }
        public DateTime? ReceptionPatientDateOfBirth { get; set; }
        public DateTime? ReceptionReceptionDate { get; set; }
        public string ReceptionReceptionNum { get; set; }

        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        public virtual ICollection<ReceptionInsuranceReceivedViewModel> ReceptionInsuranceReceiveds { get; set; }
        public virtual ICollection<ReceptionServiceReceivedViewModel> ReceptionServiceReceiveds { get; set; }
    }
}
