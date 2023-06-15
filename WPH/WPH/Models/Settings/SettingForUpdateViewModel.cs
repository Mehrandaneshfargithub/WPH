using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Settings
{
    public class SettingForUpdateViewModel
    {
        public int SettingId { get; set; }
        public String SettingName { get; set; }
        public int ValueId { get; set; }
        public String ValueName { get; set; }
        public int SettingValueId { get; set; }
        public int userProfileId { get; set; }
    }
}