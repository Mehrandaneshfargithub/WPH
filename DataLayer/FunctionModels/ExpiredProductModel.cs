using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class ExpiredProductModel
    {
        public DateTime? ExpireDate { get; set; }
        public Guid? ProductId { get; set; }
    }
}
