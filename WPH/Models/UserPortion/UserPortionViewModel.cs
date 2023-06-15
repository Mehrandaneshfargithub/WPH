using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.UserPortion
{
    public class UserPortionViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public bool? Specification { get; set; }
        public int? PortionPercent { get; set; }
        public string UserName { get; set; }
        public string UserNameAndPercent => UserName + " _ " + PortionPercent.GetValueOrDefault(0).ToString() + "%";

    }
}
