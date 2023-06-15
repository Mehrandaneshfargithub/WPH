using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;

namespace WPH.Models.ReceptionClinicSection
{
    public class ReceptionClinicSectionViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public string Description { get; set; }
        public Guid? DestinationReceptionId { get; set; }

        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
    }
}
