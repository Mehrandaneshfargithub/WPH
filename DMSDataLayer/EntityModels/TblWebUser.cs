using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWebUser
    {
        public TblWebUser()
        {
            TblWebUserCustomers = new HashSet<TblWebUserCustomer>();
            TblWebUserFormAccesses = new HashSet<TblWebUserFormAccess>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Password { get; set; }
        public string MacAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool? Active { get; set; }
        public bool? AllCutomers { get; set; }

        public virtual ICollection<TblWebUserCustomer> TblWebUserCustomers { get; set; }
        public virtual ICollection<TblWebUserFormAccess> TblWebUserFormAccesses { get; set; }
    }
}
