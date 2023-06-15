using System;
using System.ComponentModel.DataAnnotations;

namespace WPH.Models.CustomDataModels.BaseInfo
{
    public class BaseInfoViewModel : IndexViewModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int Index { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameHolder { get; set; }
        
        public Nullable<int> Priority { get; set; }
        public Nullable<Guid> TypeId { get; set; }
        public string Description { get; set; }
        public Nullable<Guid> ClinicSectionId { get; set; }
    }
}