using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Clinic
{
    public class ClinicSectionSettingFormViewModel
    {
        public List<ClinicSectionSettingValueViewModel> cssValueViewModel { get; set; }
        public List<ClinicViewModel> clinicViewModel { get; set; }
    }
}