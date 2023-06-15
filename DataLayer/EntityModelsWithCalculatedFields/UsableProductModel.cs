using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class UsableProductModel
    {
        public DateTime? ExpireDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? RemainingNum { get; set; }
        public Guid? PurchaseInvoiceDetailId { get; set; }
        public Guid? TransferDetailId { get; set; }
        public Guid? SourcePurchaseInvoiceDetailId { get; set; }
    }
}
