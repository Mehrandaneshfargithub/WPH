using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ClinicSectionSetting
    {
        public ClinicSectionSetting()
        {
            ClinicSectionSettingValues = new HashSet<ClinicSectionSettingValue>();
        }

        public int Id { get; set; }
        public string Sname { get; set; }
        public int? SectionTypeId { get; set; }
        public string InputType { get; set; }

        public virtual BaseInfoGeneral SectionType { get; set; }
        public virtual ICollection<ClinicSectionSettingValue> ClinicSectionSettingValues { get; set; }
    }
}
