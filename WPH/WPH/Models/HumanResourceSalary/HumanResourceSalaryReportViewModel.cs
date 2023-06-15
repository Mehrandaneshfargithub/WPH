using System;

namespace WPH.Models.HumanResourceSalary
{
    public class HumanResourceSalaryReportViewModel 
    {
        public string HumanName { get; set; }
        public string Date { get; set; }
        public string SalaryType { get; set; }
        public string Amount => Temp_Amount.GetValueOrDefault(0).ToString("N0");
        public string Rem => Temp_Rem.GetValueOrDefault(0).ToString("N0");
        public string Recived => Temp_Recived.GetValueOrDefault(0).ToString("N0");
        public string PaymentStatus { get; set; }
        public string Section { get; set; }
        public string ServiceName { get; set; }
        public string PatientName { get; set; }
        public string ReceptionDate { get; set; }


        public decimal? Temp_Recived { get; set; }
        public decimal? MinWorkTime { get; set; }
        public decimal? WorkTime { get; set; }
        public decimal? ExtraSalary { get; set; }
        public decimal? Salary { get; set; }
        public decimal? Temp_Amount
        {
            get
            {
                if (SalaryType?.ToLower().Equals("salary") ?? false)
                {
                    if (WorkTime < MinWorkTime)
                    {
                        return ((Salary / MinWorkTime) * WorkTime);
                    }
                    else
                    {
                        var extra = WorkTime - MinWorkTime;
                        return (Salary + (extra * ExtraSalary));
                    }
                }
                else
                {
                    return Salary;
                }
            }
        }
        public decimal? Temp_Rem => Temp_Amount.GetValueOrDefault(0) - Temp_Recived.GetValueOrDefault(0);
    }
}
