using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class Cost
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
