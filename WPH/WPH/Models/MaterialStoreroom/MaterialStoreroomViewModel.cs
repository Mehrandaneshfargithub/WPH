using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.MaterialStoreroom
{
    public class MaterialStoreroomViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string ProducerName { get; set; }
        public string ProductTypeName { get; set; }
        //public string Barcode { get; set; }
        public decimal? Count { get; set; }
        public Guid? ClinicSectionId { get; set; }
    }
}
