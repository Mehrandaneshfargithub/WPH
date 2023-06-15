using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.ReceptionInsurance;

namespace WPH.Models.ReceptionInsuranceReceived
{
    public class ReceptionInsuranceReceivedViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionInsuranceId { get; set; }
        public string PayerName { get; set; }
        public decimal? Amount { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? AmountStatus { get; set; }

        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual BaseInfoGeneralViewModel Currency { get; set; }
        public virtual ReceptionInsuranceViewModel ReceptionInsurance { get; set; }
    }
}
