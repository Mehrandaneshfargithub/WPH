using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Reception;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.CustomDataModels.PatientReceptionReceived
{
    public class PatientReceptionReceivedViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid ReceptionId { get; set; }
        public decimal? Amount { get; set; }
        public int? AmountCurrencyId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string AmountCurrencyName { get; set; }
        public string ViewDate { get; set; }
        public bool CanDelete { get; set; }
        
        public virtual BaseInfoGeneralViewModel AmountCurrency { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
    }
}