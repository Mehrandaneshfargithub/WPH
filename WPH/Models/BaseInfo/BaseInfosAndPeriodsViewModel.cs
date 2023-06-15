using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WPH.Models.BaseInfo;

namespace WPH.Models.CustomDataModels.BaseInfo
{
    public class BaseInfosAndPeriodsViewModel
    {
        public IEnumerable<BaseInfoViewModel> baseInfos { get; set; }
        public IEnumerable<PeriodsViewModel> filters { get; set; }
        public IEnumerable<PeriodsViewModel> payments { get; set; }
        public IEnumerable<PeriodsViewModel> clearances { get; set; }
        public IEnumerable<PeriodsViewModel> periods { get; set; }
        public List<SectionViewModel> sections { get; set; }
    }
}