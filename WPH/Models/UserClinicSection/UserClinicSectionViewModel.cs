using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.UserClinicSection
{
    public class UserClinicSectionViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public System.Guid ClinicSectionId { get; set; }
        public string ClinicSectionName { get; set; }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserAccessTypeName { get; set; }
    }
}