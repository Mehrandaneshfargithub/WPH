using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblPatient
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public byte[] Image { get; set; }
        public string InsuranceNumber { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int? CityId { get; set; }
    }
}
