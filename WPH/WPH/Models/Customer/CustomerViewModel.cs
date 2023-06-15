using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.Customer
{
    public class CustomerViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerTypeName { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModidiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
    }
}
