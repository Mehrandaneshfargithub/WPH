using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionDoctor
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? DoctorId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public int? DoctorRoleId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual BaseInfoGeneral DoctorRole { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
