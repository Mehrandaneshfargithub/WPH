using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class ValueOfSetting
    {
        public ValueOfSetting()
        {
            SettingValueOfSettings = new HashSet<SettingValueOfSetting>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SettingValueOfSetting> SettingValueOfSettings { get; set; }
    }
}
