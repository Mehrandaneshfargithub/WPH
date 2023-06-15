using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnPurchaseInvoiceDiscount
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
