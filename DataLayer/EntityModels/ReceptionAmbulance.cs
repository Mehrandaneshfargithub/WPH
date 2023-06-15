using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class ReceptionAmbulance
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? AmbulanceId { get; set; }
        public Guid? ReceptionId { get; set; }
        public Guid? FromHospitalId { get; set; }
        public Guid? ToHospitalId { get; set; }
        public decimal? Cost { get; set; }
        public int? CostCurrencyId { get; set; }
        public int? PatientHealthId { get; set; }
        public string Explanation { get; set; }
        public bool? Active { get; set; }

        public virtual Ambulance Ambulance { get; set; }
        public virtual BaseInfoGeneral CostCurrency { get; set; }
        public virtual Hospital FromHospital { get; set; }
        public virtual BaseInfoGeneral PatientHealth { get; set; }
        public virtual Reception Reception { get; set; }
        public virtual Hospital ToHospital { get; set; }
    }
}
