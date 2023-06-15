using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.CustomDataModels.Patient;

namespace WPH.Models.CustomDataModels.PatientMedicine
{
    public class PatientMedicineRecordViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid Patientid { get; set; }
        public Guid MedicineId { get; set; }
        public string MedicineName { get; set; }
        public virtual MedicineViewModel Medicine { get; set; }
        public virtual PatientViewModel Patient { get; set; }
    }
}