using System;

namespace WPH.Models.Cash
{
    public class DoctorWageViewModel
    {
        public Guid ReceptionServiceId { get; set; }

        public Guid ReceptionId { get; set; }

        public Guid DoctorGuid { get; set; }
        public string DoctorName { get; set; }
        public decimal? Amount { get; set; }

        public Guid AnesthesiologistGuid { get; set; }
        public string Anesthesiologist { get; set; }
        public decimal? AnesthesiologistAmount { get; set; }

        public Guid PediatricianGuid { get; set; }
        public string Pediatrician { get; set; }
        public decimal? PediatricianAmount { get; set; }

        public Guid ResidentGuid { get; set; }
        public string Resident { get; set; }
        public decimal? ResidentAmount { get; set; }

        public Guid UserId { get; set; }
        public string CadreType { get; set; }

        public Guid ServiceId { get; set; }
        public decimal? HospitalWage { get; set; }
        public string Decription { get; set; }
    }
}
