using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;

namespace WPH.Models.ReceptionTemperature
{
    public class ReceptionTemperatureViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? PulseRate { get; set; }
        public decimal? SYSBloodPressure { get; set; }
        public decimal? DIABloodPressure { get; set; }
        public decimal? RespirationRate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? InsertedDate { get; set; }
        public string TemperatureDateDay { get; set; }
        public string TemperatureDateMonth { get; set; }
        public string TemperatureDateYear { get; set; }
        public string InsertedTime { get; set; }

        public virtual ReceptionViewModel Reception { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
    }
}
