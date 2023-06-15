using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Views.Shared.PartialViews.AppWebForms.Home;

namespace WPH.Models.Dashboard
{
    public class HospitalDashboardViewModel
    {
        public string RoomName { get; set; }
        public string BedName { get; set; }
        public IEnumerable<BedInfoViewModel> RoomInfo { get; set; }
        public BedInfoViewModel SingleRoomInfo { get; set; }


    }
}
