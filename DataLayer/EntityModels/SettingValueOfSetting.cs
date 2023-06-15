using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class SettingValueOfSetting
    {
        public SettingValueOfSetting()
        {
            UserProfiles = new HashSet<UserProfile>();
        }

        public int Id { get; set; }
        public int SettingId { get; set; }
        public int ValueId { get; set; }

        public virtual Setting Setting { get; set; }
        public virtual ValueOfSetting Value { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
