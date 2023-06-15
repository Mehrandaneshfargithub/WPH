using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnSaleInvoice
    {
        [NotMapped]
        public string TotalDiscount { get; set; }
    }
}
