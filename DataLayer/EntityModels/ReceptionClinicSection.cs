using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionClinicSection
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public string Description { get; set; }
        public Guid? DestinationReceptionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual Reception DestinationReception { get; set; }
        public virtual Reception Reception { get; set; }
    }
}
