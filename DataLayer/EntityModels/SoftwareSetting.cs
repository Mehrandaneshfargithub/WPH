using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class SoftwareSetting
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string Value { get; set; }
    }
}
