using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Visit
{
    public class PayVisitViewModel
    {
        public Guid? ReceptionId { get; set; }
        public decimal? VisitPrice { get; set; }
        public Guid CreatedUserId { get; set; }
    }
}
