using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Clinic
{
    public class ClinicSectionSettingValueViewModel : IndexViewModel
    {

        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int SettingId { get; set; }
        public string ClinicSectionSettingSName { get; set; }
        public string ShowSName { get; set; }
        public string SValue { get; set; }
        
    }
}