using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionTemperature
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? PulseRate { get; set; }
        public decimal? SYSBloodPressure { get; set; }
        public decimal? DIABloodPressure { get; set; }
        public decimal? RespirationRate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? InsertedDate { get; set; }

        public virtual Reception Reception { get; set; }
        public virtual User CreatedUser { get; set; }
    }

}
