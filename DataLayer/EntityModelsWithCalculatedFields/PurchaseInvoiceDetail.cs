using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoiceDetail
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
