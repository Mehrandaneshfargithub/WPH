using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.BaseInfo
{
    public class BaseInfoTypeViewModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string EName { get; set; }
        public string FName { get; set; }
    }
}