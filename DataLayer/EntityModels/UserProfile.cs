using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class UserProfile
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int SettingId { get; set; }
        public int SettingValueId { get; set; }

        public virtual Setting Setting { get; set; }
        public virtual SettingValueOfSetting SettingValue { get; set; }
        public virtual User User { get; set; }
    }
}
