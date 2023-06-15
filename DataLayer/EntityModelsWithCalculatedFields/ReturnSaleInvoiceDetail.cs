using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnSaleInvoiceDetail
    {
        [NotMapped]
        public string CurrencyName { get; set; }
    }
}
