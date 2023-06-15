using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ProductBarcode
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ProductId { get; set; }
        public string Barcode { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User CreateUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual Product Product { get; set; }
    }
}
