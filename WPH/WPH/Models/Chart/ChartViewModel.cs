using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;

namespace WPH.Models.CustomDataModels.Chart
{
    public class ChartViewModel
    {
        public Nullable<decimal> Value { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public List<List<ChartViewModel>> AllValues { get; set; }
        public IEnumerable<ClinicSectionChoosenValueViewModel> AllClinicSectionChoosenValues { get; set; }
        public List<string> Category { get; set; }
    }
}