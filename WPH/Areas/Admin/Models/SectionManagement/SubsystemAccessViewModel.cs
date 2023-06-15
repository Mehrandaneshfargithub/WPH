using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Areas.Admin.Models.SectionManagement
{
    public class SubsystemAccessViewModel
    {
        public int SubSystemId { get; set; }
        public int AccessId { get; set; }
        public int SectionTypeId { get; set; }
        public string AccessName { get; set; }
        public bool Checked { get; set; }
    }
}
