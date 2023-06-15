using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class PrescriptionTestDetail
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? TestId { get; set; }
        public string Explanation { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid? ReceptionId { get; set; }
        public string AnalysisName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual BaseInfo Test { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
    }
}
