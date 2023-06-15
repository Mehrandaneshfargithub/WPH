using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Product
{
    public class ProductStoreroomViewModel : IndexViewModel
    {
        private bool warning;
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public string ChildrensGuid { get; set; }
        public int ChildrensCount { get; set; }
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string ProducerName { get; set; }
        public string ProductTypeName { get; set; }
        public decimal? Count { get; set; }
        public int? OrderPoint { get; set; }
        public string ProductLocation { get; set; }
        public string Description { get; set; }
        public Guid? ProductMasterId { get; set; }
        public bool Warning
        {
            set { warning = value; }
            get { return warning && OrderPoint != null && Count <= OrderPoint; }
        }
    }
}
