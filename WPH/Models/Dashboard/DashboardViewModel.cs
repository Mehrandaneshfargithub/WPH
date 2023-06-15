using System;
using System.Collections.Generic;

namespace WPH.Models.CustomDataModels.Dashboard
{
    public class DashboardViewModel
    {
        
        public DateTime Date { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Amount { get; set; }
        public List<string> DiseaseName { get; set; }
        public List<string> MedicineName { get; set; }
        public List<string> MedicineType { get; set; }
    }
}