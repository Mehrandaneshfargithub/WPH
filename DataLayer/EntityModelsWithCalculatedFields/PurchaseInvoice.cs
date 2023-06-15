using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EntityModels
{
    public partial class PurchaseInvoice
    {
        [NotMapped]
        public string TotalDiscount { get; set; }
        [NotMapped]
        public bool Status { get; set; }
    }
}
