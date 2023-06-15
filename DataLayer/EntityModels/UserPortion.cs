using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class UserPortion
    {
        public UserPortion()
        {
            ReceptionDetailPays = new HashSet<ReceptionDetailPay>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public bool? Specification { get; set; }
        public int? PortionPercent { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<ReceptionDetailPay> ReceptionDetailPays { get; set; }
    }
}
