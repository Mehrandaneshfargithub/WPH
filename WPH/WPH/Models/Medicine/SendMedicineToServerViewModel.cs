using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Medicine
{
    public class SendMedicineToServerViewModel
    {
        public string MedicineName { get; set; }
        public string Number { get; set; }
        public string ConsumptionInstruction { get; set; }
        public string Explanation { get; set; }
        public string MedicineForm { get; set; }
    }
}
