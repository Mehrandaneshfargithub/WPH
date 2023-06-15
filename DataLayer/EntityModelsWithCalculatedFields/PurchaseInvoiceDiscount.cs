using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoiceDiscount
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
