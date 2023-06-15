using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.SectionManagement
{
    public class ClinicSectionNamesViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public int? SectionTypeId { get; set; }
        public string SectionTypeName { get; set; }
        public int? ClinicSectionTypeId { get; set; }
        public string ClinicSectionTypeName { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? UserId { get; set; }
        public string ParentName { get; set; }
        public int? Priority { get; set; }
    }
}
