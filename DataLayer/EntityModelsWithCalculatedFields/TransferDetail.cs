using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class TransferDetail
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
