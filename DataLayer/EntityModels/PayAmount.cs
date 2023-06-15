using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class PayAmount
    {
        public Guid Guid { get; set; }
        public Guid PayId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Discount { get; set; }
        public int? BaseCurrencyId { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? BaseAmount { get; set; }
        public decimal? DestAmount { get; set; }

        public virtual BaseInfoGeneral BaseCurrency { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual Pay Pay { get; set; }
    }
}
