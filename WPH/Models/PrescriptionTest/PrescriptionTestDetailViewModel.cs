using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.PrescriptionTest
{
    public class PrescriptionTestDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public System.Guid Guid { get; set; }
        public int Id { get; set; }
        public Nullable<System.Guid> VisitId { get; set; }
        public Nullable<System.Guid> TestId { get; set; }
        public string Explanation { get; set; }
        public System.Guid ClinicSectionId { get; set; }
        public string TestName { get; set; }
        public string AnalysisName { get; set; }
        public string ModifiedUserName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual VisitViewModel Visit { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
    }
}