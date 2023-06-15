using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Patient;
using WPH.Models.CustomDataModels.Reserve;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.ReserveDetail
{
    public class ReserveDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid MasterId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? SecretaryId { get; set; }
        public Guid? DoctorId { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }
        public TimeSpan? ReservedTime { get; set; }
        public string Explanation { get; set; }
        public string ReserveStartTime { get; set; }
        public string ReserveEndTime { get; set; }
        public bool? LastVisit { get; set; }
        public bool? UseFormNum { get; set; }
        public bool? UseAutoCompleteFormNum { get; set; }
        public DateTime? ReserveDate { get; set; }
        public decimal Amount { get; set; }
        public string Remain { get; set; }
        public bool? OldVisit { get; set; }
        public Guid UserId { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid OriginalClinicSectionId { get; set; }

        public virtual BaseInfoGeneralViewModel Status { get; set; }
        public virtual ReserveViewModel Reserve { get; set; }
        public virtual PatientViewModel Patient { get; set; }
        public virtual ICollection<VisitViewModel> Visits { get; set; }
    }
}