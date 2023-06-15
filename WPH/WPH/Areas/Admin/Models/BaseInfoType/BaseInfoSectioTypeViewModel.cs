using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Areas.Admin.Models.BaseInfoType
{
    public class BaseInfoSectioTypeViewModel
    {
        public Guid? BaseInfoTypeId { get; set; }
        public int? SectionTypeId { get; set; }
        public string SectionTypeName { get; set; }
        public bool Checked { get; set; }
    }
}
