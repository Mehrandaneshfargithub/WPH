using System;

namespace DataLayer.EntityModels
{
    public class ReceptionForCashReport
    {
        public Guid ReceptionId { get; set; }
        public string VName { get; set; }
        public string VType { get; set; }
        public decimal? Price { get; set; }
        public string StatusName { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public int? SurgeryId { get; set; }
        public string Description { get; set; }
    }
}
