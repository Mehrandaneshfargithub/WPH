using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class DamageDiscount
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
