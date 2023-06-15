using System;
using WPH.Models.CustomDataModels;

namespace WPH.Models.Service
{
    public class ServiceViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public Guid ClinicSectionId { get; set; }
        public int? Priority { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public decimal? DoctorWage { get; set; }
        public string OperationTypeName { get; set; }
    }
}
