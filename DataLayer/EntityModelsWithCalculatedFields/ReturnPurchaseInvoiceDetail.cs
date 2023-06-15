using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnPurchaseInvoiceDetail
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
