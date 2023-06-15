using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class UserPortionReport
    {
        public Guid UserPortionId { get; set; }
        public DateTime ReceptionDate { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public decimal ServiceAmount { get; set; }
        public virtual ICollection<Service> Service { get; set; }
        public decimal Amount { get; set; }
    }
}
