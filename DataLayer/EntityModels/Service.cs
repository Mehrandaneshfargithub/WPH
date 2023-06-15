using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Service
    {
        public Service()
        {
            ReceptionServices = new HashSet<ReceptionService>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Price { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? Priority { get; set; }
        public int? TypeId { get; set; }
        public decimal? DoctorWage { get; set; }
        public Guid? OperationTypeId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
        public virtual BaseInfoGeneral Currency { get; set; }
        public virtual BaseInfo OperationType { get; set; }
        public virtual BaseInfoGeneral Type { get; set; }
        public virtual ICollection<ReceptionService> ReceptionServices { get; set; }
    }
}
