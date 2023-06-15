using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblAnnotation
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public int UserId { get; set; }
        public int AnnoType { get; set; }
        public DateTime Date { get; set; }
        public string AutoAnnotation { get; set; }
        public string TextAnnotation { get; set; }
        public DateTime ReminderDate { get; set; }
        public bool IsShown { get; set; }
        public bool? IsReminder { get; set; }
        public string SourceType { get; set; }
        public bool? IsShowing { get; set; }
        public int? SupplierId { get; set; }
        public bool? IsSupplierAccount { get; set; }
        public bool? IsPurchaseInvoice { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsCustomerAccount { get; set; }
        public bool? IsSaleInvoice { get; set; }
        public int? ReminderDuration { get; set; }
        public int? ReminderRepeatNumber { get; set; }
        public int? ReminderRepeatCurrentNumber { get; set; }
    }
}
