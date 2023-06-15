using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PrescriptionDetail
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid MedicineId { get; set; }
        public string Num { get; set; }
        public string ConsumptionInstruction { get; set; }
        public string Explanation { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual Medicine Medicine { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
    }
}
