using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.ClinicSection
{
    public class ClinicSectionViewModel: IndexViewModel
    {
        public System.Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public System.Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string Explanation { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string SystemCode { get; set; }
        public string SystemCodeHolder { get; set; }
        public string licenseNumber { get; set; }
        public int? SectionTypeId { get; set; }
        public Guid? ParentId { get; set; }
        public string SectionTypeName { get; set; }
        public int? ClinicSectionTypeId { get; set; }
        public string ClinicSectionTypeName { get; set; }
    }
}