using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.ProductBarcode
{
    public class ProductBarcodeViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public Guid? ProductId { get; set; }
        public string Barcode { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid ClinicSectionId { get; set; }
    }
}
