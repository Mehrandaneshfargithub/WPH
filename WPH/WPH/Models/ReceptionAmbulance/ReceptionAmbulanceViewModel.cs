using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Ambulance;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Hospital;
using WPH.Models.Reception;

namespace WPH.Models.ReceptionAmbulance
{
    public class ReceptionAmbulanceViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? AmbulanceId { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? FromHospitalId { get; set; }
        public string FromHospitalName { get; set; }
        public Guid? ToHospitalId { get; set; }
        public string ToHospitalName { get; set; }
        public decimal? Cost { get; set; }
        public int? CostCurrencyId { get; set; }
        public int? PatientHealthId { get; set; }
        public string Explanation { get; set; }
        public bool? Active { get; set; }

        public virtual AmbulanceViewModel Ambulance { get; set; }
        public virtual BaseInfoGeneralViewModel CostCurrency { get; set; }
        public virtual HospitalViewModel FromHospital { get; set; }
        public virtual BaseInfoGeneralViewModel PatientHealth { get; set; }
        public virtual ReceptionViewModel Reception { get; set; }
        public virtual HospitalViewModel ToHospital { get; set; }
    }
}
