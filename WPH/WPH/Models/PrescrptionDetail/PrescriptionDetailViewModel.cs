using System;
using System.Web;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.CustomDataModels.Visit;

namespace WPH.Models.CustomDataModels.PrescrptionDetail
{
    public class PrescriptionDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid VisitId { get; set; }
        public Guid MedicineId { get; set; }
        public string Num { get; set; }
        public string ConsumptionInstruction { get; set; }
        public string Explanation { get; set; }
        public string MedicineJoineryName { get; set; }
        public string ModifiedUserName { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual MedicineViewModel Medicine { get; set; }
        public virtual VisitViewModel Visit { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
    }
}