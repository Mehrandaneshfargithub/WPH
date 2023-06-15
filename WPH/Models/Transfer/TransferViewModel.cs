using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Transfer
{
    public class TransferViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateTxt { get; set; }
        public Guid? SourceClinicSectionId { get; set; }
        public string SourceClinicSectionName { get; set; }
        public Guid? DestinationClinicSectionId { get; set; }
        public string DestinationClinicSectionName { get; set; }
        public string Description { get; set; }
        public string ReceiverName { get; set; }
        public Guid? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string ReceiverUserName { get; set; }
        public Guid? ClinicSectionId { get; set; }

    }
}
