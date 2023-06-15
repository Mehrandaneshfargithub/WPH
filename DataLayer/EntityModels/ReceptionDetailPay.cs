using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionDetailPay
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Amount { get; set; }
        public Guid? UserPortionId { get; set; }

        public virtual Reception Reception { get; set; }
        public virtual UserPortion UserPortion { get; set; }
    }
}
