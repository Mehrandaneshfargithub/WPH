using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class DamageDetail
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
