using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.UserManagment
{
    public class SubSystemsWithAccessViewModel
    {
        public int Id { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public bool Checked { get; set; }
    }
}