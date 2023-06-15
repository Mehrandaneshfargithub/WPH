using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.BaseInfoType
{
    public class BaseInfoTypeViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Ename { get; set; }
        public string Fname { get; set; }
    }
}
