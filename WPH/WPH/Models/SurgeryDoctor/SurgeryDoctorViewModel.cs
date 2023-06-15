using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.Surgery;

namespace WPH.Models.SurgeryDoctor
{
    public class SurgeryDoctorViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int? DoctorRoleId { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? SurgeryId { get; set; }
        public string Explanation { get; set; }

        public virtual DoctorViewModel Doctor { get; set; }
        public virtual BaseInfoGeneralViewModel DoctorRole { get; set; }
        //public virtual SurgeryViewModel Surgery { get; set; }
    }
}
