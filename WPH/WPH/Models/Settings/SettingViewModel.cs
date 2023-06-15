using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Settings
{
    public class SettingViewModel
    {
        public Guid UserId { get; set; }
        public String SettingName { get; set; }
        public String ValueName { get; set; }
    }
}