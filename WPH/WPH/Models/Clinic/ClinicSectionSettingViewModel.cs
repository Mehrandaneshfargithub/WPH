using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Clinic
{
    public class ClinicSectionSettingViewModel
    {
        public int Id { get; set; }
        public string SName { get; set; }
        public string InputType { get; set; }

        public ICollection<ClinicSectionSettingValueViewModel> ClinicSectionSettingValues { get; set; }
    }
}