using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.TransferDetail
{
    public class TransferDetailViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? MasterId { get; set; }
        public decimal? Num { get; set; }
        public string Consideration { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductName { get; set; }
        public Guid? DestinationProductId { get; set; }
        public string DestinationProductName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ExpireDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public bool Is_Parent { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid DetailId { get; set; }
        public string InvoiceType { get; set; }
    }
}
