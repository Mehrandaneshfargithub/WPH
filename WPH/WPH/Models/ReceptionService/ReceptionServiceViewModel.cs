using System;
using WPH.Models.CustomDataModels;
using WPH.Models.Service;

namespace WPH.Models.ReceptionService
{
    public class ReceptionServiceViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ReceptionId { get; set; }
        public decimal? Number { get; set; }
        public int? StatusId { get; set; }
        public string ServiceStatus { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? Recived { get; set; }
        public decimal? Returned { get; set; }
        public int? DiscountCurrencyId { get; set; }
        public Guid? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ServiceName { get; set; }
        public DateTime? ServiceDate { get; set; }
        public string ServiceDateDay { get; set; }
        public string ServiceDateMonth { get; set; }
        public string ServiceDateYear { get; set; }
        public string ServiceDateHour { get; set; }
        public string ServiceDateMin { get; set; }
        public Guid? ProductId { get; set; }
        public int? ProductIdDMS { get; set; }
        public string ProductName { get; set; }
        public string ReceptionInvoiceNum { get; set; }
        public string Explanation { get; set; }
        public decimal? PayByInsurance { get; set; }
        public bool ShowPay { get; set; }
        public bool ShowDelete { get; set; }


        public decimal? Total => (Number * Price) - (Discount.GetValueOrDefault(0));
        public decimal? Rem => Total - (Recived - Returned);

        public virtual ServiceViewModel Service { get; set; }
    }
}
