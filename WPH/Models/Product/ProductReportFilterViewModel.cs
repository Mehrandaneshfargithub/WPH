using System;

namespace WPH.Models.Product
{
    public class ProductReportFilterViewModel
    {
        public Guid ClinicSectionId { get; set; }
        public Guid ProductId { get; set; }
        public int? CurrencyId { get; set; }
        public int Year { get; set; }
        public DateTime DateFrom { get; set; }
        public string DateFromTxt { get; set; }
        public DateTime DateTo { get; set; }
        public string DateToTxt { get; set; }
    }
}
