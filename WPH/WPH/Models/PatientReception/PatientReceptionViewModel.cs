using System;
using System.Globalization;
using WPH.Models.CustomDataModels.Patient;

namespace WPH.Models.CustomDataModels.PatientReception
{
    public class PatientReceptionViewModel : IndexViewModel
    {
        CultureInfo cultures = new CultureInfo("en-US");

        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? Remained { get; set; }

        public Guid? RoomBedId { get; set; }
        public string RoomBedName { get; set; }
        public string ReceptionStatus { get; set; }
        public decimal? TotalServiceAmount { get; set; }
        public decimal? TotalRecivedAmount { get; set; }
        public decimal? TotalReturnedAmount { get; set; }
        public string PurposeName { get; set; }
        public decimal? Insurance { get; set; }

        public decimal? Rem => TotalServiceAmount - (TotalRecivedAmount - TotalReturnedAmount);
        public string NameMobile => $"{Patient?.User?.Name} _ {Patient?.User?.PhoneNumber}";
        public string Invoice => $"{InvoiceNum} _ {InvoiceDate.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy", cultures) }";

        public virtual PatientViewModel Patient { get; set; }
    }
}