using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.Models.Supplier
{
    public class SupplierViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? SupplierTypeId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string SupplierTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public int? CurrencyId { get; set; }
        public string PhoneNumber { get; set; }
        public decimal? Discount { get; set; }
        public decimal? WholePurchase { get; set; }
        public decimal? WholePay { get; set; }
        public decimal? WholePurchaseAfterDiscount { get; set; }
        public decimal? remain { get { return WholePurchaseAfterDiscount.GetValueOrDefault(0) - WholePay.GetValueOrDefault(0); } }

        public virtual BaseInfoViewModel City { get; set; }
        public virtual BaseInfoViewModel Country { get; set; }
        public virtual BaseInfoViewModel SupplierType { get; set; }
        public virtual UserInformationViewModel User { get; set; }
        public virtual ClinicSectionViewModel ClinicSection { get; set; }
        public virtual UserInformationViewModel CreatedUser { get; set; }
        public virtual UserInformationViewModel ModifiedUser { get; set; }
    }
}
