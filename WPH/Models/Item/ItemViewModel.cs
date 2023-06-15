using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Item
{
    public class ItemViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid? ItemTypeId { get; set; }
        public string NameHolder { get; set; }
        public string ItemTypeName { get; set; }
        public Guid? SectionId { get; set; }
        public string SectionName { get; set; }
        public Guid? ClinicSectionId { get; set; }
    }
}