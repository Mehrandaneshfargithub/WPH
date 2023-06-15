using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.AdminClinicSectionSetting
{
    public class AdminClinicSectionSettingViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Sname { get; set; }
        public int? SectionTypeId { get; set; }
        public string SectionTypeName { get; set; }
        public string InputType { get; set; }
    }
}
