using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.SectionManagement
{
    public class SubsystemViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public int? Parent { get; set; }
        public string ParentName { get; set; }
        public string Link { get; set; }
        public string ShowName { get; set; }
        public bool? Active { get; set; }
        public string Icon { get; set; }
    }
}
