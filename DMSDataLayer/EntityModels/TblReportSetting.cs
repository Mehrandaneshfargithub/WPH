using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblReportSetting
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameLatin { get; set; }
        public string Desc { get; set; }
        public string DescLation { get; set; }
        public string Address { get; set; }
        public string AddressLatin { get; set; }
        public string Mob { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string LastReportStatement { get; set; }
    }
}
