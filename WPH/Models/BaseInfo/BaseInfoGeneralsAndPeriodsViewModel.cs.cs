using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;

namespace WPH.Models.BaseInfo
{
    public class BaseInfoGeneralsAndPeriodsViewModel
    {
        public IEnumerable<BaseInfoGeneralViewModel> baseInfoGenerals { get; set; }
        public IEnumerable<PeriodsViewModel> periods { get; set; }
    }
}
