using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnSaleInvoiceDiscount
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
