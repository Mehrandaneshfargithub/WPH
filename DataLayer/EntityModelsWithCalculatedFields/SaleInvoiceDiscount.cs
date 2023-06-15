using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class SaleInvoiceDiscount
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
