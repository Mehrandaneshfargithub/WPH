using System;
using WPH.Models.CustomDataModels;
using WPH.Models.PurchaseInvoiceDetail;

namespace WPH.Models.PurchaseInvoice
{
    public class PurchaseInvoiceViewModel : IndexViewModel
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
        public string MainInvoiceNum { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public string PayIds { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalCost { get; set; }
        public string Status { get; set; }
        public bool CanChange { get; set; }
        public string PurchaseType { get; set; }

        public PurchaseInvoiceDetailViewModel PurchaseInvoiceDetails { get; set; }
    }
}
