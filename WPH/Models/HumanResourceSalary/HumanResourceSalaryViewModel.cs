using System;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.HumanResource;

namespace WPH.Models.HumanResourceSalary
{
    public class HumanResourceSalaryViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public Guid? HumanResourceId { get; set; }
        public string HumanResourceName { get; set; }
        public DateTime? BeginDate { get; set; }
        public string Begin_Date { get; set; }
        public DateTime? EndDate { get; set; }
        public string End_Date { get; set; }
        public decimal? WorkTime { get; set; }
        public decimal? ExtraSalary { get; set; }
        public decimal? Salary { get; set; }
        public int? CurrencyId { get; set; }
        public Guid? CreateUserId { get; set; }
        public Guid? ModifiedUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? SalaryTypeId { get; set; }
        public int? SalaryTypeHolderId { get; set; }
        public string SalaryType { get; set; }
        public string InvoiceNum { get; set; }
        public string DisplaySalaryType
        {
            get
            {
                if (SalaryType?.ToLower().Equals("wage") ?? false)
                {
                    return $"{SalaryType} | {InvoiceNum}";
                }
                else
                {
                    return SalaryType;
                }
            }
        }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatus { get; set; }
        public decimal? RecivedMoney { get; set; }
        public Guid? SurgeryId { get; set; }
        public decimal? Amount
        {
            get
            {
                if (SalaryType?.ToLower().Equals("salary") ?? false)
                {
                    if (WorkTime < HumanResource.MinWorkTime)
                    {
                        return ((Salary / HumanResource.MinWorkTime) * WorkTime);
                    }
                    else
                    {
                        var extra = WorkTime - HumanResource.MinWorkTime;
                        return (Salary + (extra * ExtraSalary));
                    }
                }
                else
                {
                    return Salary;
                }
            }
        }

        public decimal? Rem => Amount.GetValueOrDefault(0) - RecivedMoney.GetValueOrDefault(0);

        public virtual BaseInfoGeneralViewModel Currency { get; set; }
        public virtual HumanResourceViewModel HumanResource { get; set; }
    }
}
