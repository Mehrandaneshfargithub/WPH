using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class UserPortionExceptPaysModel
    {
        public Guid? UserId { get; set; }
        public int? PortionPercent { get; set; }
        public string UserName { get; set; }
    }
}
