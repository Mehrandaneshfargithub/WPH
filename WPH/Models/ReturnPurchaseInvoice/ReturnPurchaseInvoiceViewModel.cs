using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.PurchaseInvoiceDetail;

namespace WPH.Models.ReturnPurchaseInvoice
{
    public class ReturnPurchaseInvoiceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateTxt { get; set; }
        public Guid? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public string TotalDiscount { get; set; }
        public string PayIds { get; set; }
        public bool CanChange { get; set; }
    }
}
