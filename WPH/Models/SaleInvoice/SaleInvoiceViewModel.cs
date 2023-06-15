using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.Customer;
using WPH.Models.SaleInvoiceCost;
using WPH.Models.SaleInvoiceDetail;
using WPH.Models.SaleInvoiceDiscount;

namespace WPH.Models.SaleInvoice
{
    public class SaleInvoiceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string InvoiceNum { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceDateTxt { get; set; }
        public string Description { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ClinicSectionId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? SalePriceTypeId { get; set; }
        public string TotalPrice { get; set; }
        //public string TotalPriceAfterDiscount => SaleInvoiceDetails.Sum(a=>a.)
        public bool? OldFactor { get; set; }
        public string CustomerName { get; set; }
        public string SalePriceTypeName { get; set; }
        public string Status { get; set; }

        public virtual CustomerViewModel Customer { get; set; }
        public virtual BaseInfoGeneralViewModel SalePriceType { get; set; }
        public virtual ICollection<SaleInvoiceCostViewModel> SaleInvoiceCosts { get; set; }
        public virtual ICollection<SaleInvoiceDetailViewModel> SaleInvoiceDetails { get; set; }
        public virtual ICollection<SaleInvoiceDiscountViewModel> SaleInvoiceDiscounts { get; set; }
    }
}
