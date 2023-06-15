using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Setting
    {
        public Setting()
        {
            SettingValueOfSettings = new HashSet<SettingValueOfSetting>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SettingValueOfSetting> SettingValueOfSettings { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
