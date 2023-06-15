using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models
{
    public class HierarchyViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public int? parentId { get; set; }
        public long? population { get; set; }

        public string flagUrl { get; set; }

        public bool @checked { get; set; }

        public bool haschildren { get; set; }
        public virtual List<HierarchyViewModel> children { get; set; }
          

        
    }
}