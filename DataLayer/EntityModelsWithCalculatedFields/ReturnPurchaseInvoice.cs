using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class ReturnPurchaseInvoice
    {
        [NotMapped]
        public string TotalDiscount { get; set; }
    }
}
