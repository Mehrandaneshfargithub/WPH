using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Medicine
{
    public class GetMedicineFromServerViewModel
    {
        public Guid Guid { get; set; }
        public string MedicineName { get; set; }
        public int Number { get; set; }
        public string ConsumptionInstruction { get; set; }
        public string Explanation { get; set; }
        public string MedicineForm { get; set; }
        public int Recived { get; set; }
        public int Rem => Number - Recived;
    }
}
