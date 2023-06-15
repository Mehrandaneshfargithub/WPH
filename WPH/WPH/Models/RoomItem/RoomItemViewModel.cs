using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Disease;

namespace WPH.Models.RoomItem
{
    public class RoomItemViewModel : IndexViewModel
    {
        public int Index { get; set; }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? ItemId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public string RoomName { get; set; }
        public string ItemName { get; set; }
        public string ItemTypeName { get; set; }
        public Guid? RoomIdHolder { get; set; }
        public Guid? ItemHolder { get; set; }
    }
}