using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Damage
{
    public class DamageViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateTxt { get; set; }
        public string Description { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public string TotalPrice { get; set; }
        public string CostTypeTxt { get; set; }
        public string ReasonTxt { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public string TotalDiscount { get; set; }
    }
}
