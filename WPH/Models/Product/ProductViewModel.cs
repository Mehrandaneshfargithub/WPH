using System;
using System.Collections.Generic;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string NameHolder { get; set; }
        public Guid? ProducerId { get; set; }
        public string ProducerName { get; set; }
        public string Barcode { get; set; }
        public Guid? ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ProductMasterId { get; set; }
        public decimal? Stock { get; set; }
        public string Description { get; set; }
        public string ClinicSections { get; set; }
        public Guid[] ClinicSectionIds { get; set; }
        public IEnumerable<string> ClinicSectionss { get; set; }

        public Guid? ClinicSectionId { get; set; }
        public int? MaterialTypeId { get; set; }
        public int? OrderPoint { get; set; }
        public string ProductLocation { get; set; }
        public string FullName => $"{Name} | {ProductTypeName} | {ProducerName}";
    }
}