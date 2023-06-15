using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class Surgery
    {
        public Surgery()
        {
            HumanResourceSalaries = new HashSet<HumanResourceSalary>();
            SurgeryDoctors = new HashSet<SurgeryDoctor>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? SurgeryRoomId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string SurgeryDetail { get; set; }
        public string Explanation { get; set; }
        public string PostOperativeTreatment { get; set; }
        public string SideEffects { get; set; }
        public int? AnesthesiologistionTypeId { get; set; }
        public string AnesthesiologistionMedicine { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? SurgeryDate { get; set; }
        public int? ClassificationId { get; set; }

        public virtual BaseInfoGeneral AnesthesiologistionType { get; set; }
        public virtual BaseInfoGeneral Classification { get; set; }
        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual ICollection<HumanResourceSalary> HumanResourceSalaries { get; set; }
        public virtual ICollection<SurgeryDoctor> SurgeryDoctors { get; set; }
    }
}
