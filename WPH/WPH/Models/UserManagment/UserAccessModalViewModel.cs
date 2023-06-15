using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPH.Models.CustomDataModels.Language;

namespace WPH.Models.CustomDataModels.UserManagment
{
    public class UserAccessModalViewModel
    {
        public List<SubSystemViewModel> allOfSubSystems { get; set; }
        public List<SubSystemViewModel> oneOfUsersSubSystem { get; set; }
        public Guid userId { get; set; }
    }
}