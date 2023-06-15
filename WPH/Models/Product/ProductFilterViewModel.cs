using System;

namespace WPH.Models.Product
{
    public class ProductFilterViewModel
    {
        public Guid OriginalClinicSectionId { get; set; }
        public Guid ClinicSectionId { get; set; }
        public Guid? SupplierId { get; set; }
        public string ProductBarcode { get; set; }
        public string ClinicSectionName { get; set; }
        public int? FromOrderPoint { get; set; }
        public int? ToOrderPoint { get; set; }
    }
}
