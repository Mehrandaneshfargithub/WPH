using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Reception;
using WPH.Models.CustomDataModels.Doctor;

namespace WPH.Models.ReceptionDoctor
{
    public class ReceptionDoctorViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? DoctorId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public int? DoctorRoleId { get; set; }
        public string DoctorRoleName { get; set; }

        public virtual DoctorViewModel Doctor { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
    }
}
