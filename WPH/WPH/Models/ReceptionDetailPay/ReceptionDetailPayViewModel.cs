using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Reception;

namespace WPH.Models
{
    public class ReceptionDetailPayViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Amount { get; set; }
        public Guid? UserPortionId { get; set; }
        public string UserPortionUserName { get; set; }

        
    }
}
