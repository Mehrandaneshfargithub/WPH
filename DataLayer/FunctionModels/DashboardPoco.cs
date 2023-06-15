using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class DashboardPoco
    {
        public Nullable<DateTime> Date { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public List<string> DiseaseName { get; set; }
        public List<string> MedicineName { get; set; }
        public List<string> MedicineType { get; set; }
    }
}
