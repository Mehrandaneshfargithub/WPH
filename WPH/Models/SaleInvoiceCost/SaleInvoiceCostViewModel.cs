using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.SaleInvoiceCost
{
    public class SaleInvoiceCostViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public DateTime? CostDate { get; set; }
        public decimal? Price { get; set; }
        public string Explanation { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? SaleInvoiceId { get; set; }
        public Guid? UserId { get; set; }

        public virtual BaseInfoGeneralViewModel Currency { get; set; }
        public virtual UserInformationViewModel User { get; set; }
    }
}
