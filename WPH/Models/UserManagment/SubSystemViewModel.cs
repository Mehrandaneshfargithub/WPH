using System;

namespace WPH.Models.CustomDataModels.UserManagment
{
    public class SubSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> Priority { get; set; } 
        public Nullable<int> Parent { get; set; }
        public string Link { get; set; }
        public string ShowName { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Icon { get; set; }
        public string AccessName { get; set; }
        public string AccessShowName { get; set; }
    }
}