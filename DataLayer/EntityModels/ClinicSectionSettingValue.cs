using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ClinicSectionSettingValue
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int SettingId { get; set; }
        public string Svalue { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual ClinicSectionSetting Setting { get; set; }
    }
}
