using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class SurgeryDoctor
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? DoctorId { get; set; }
        public Guid? SurgeryId { get; set; }
        public string Explanation { get; set; }
        public int? DoctorRoleId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual BaseInfoGeneral DoctorRole { get; set; }
        public virtual Surgery Surgery { get; set; }
    }
}
