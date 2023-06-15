using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblWebFormtblAccess
    {
        public TblWebFormtblAccess()
        {
            TblWebFormAccessAccesses = new HashSet<TblWebFormAccess>();
            TblWebFormAccessForms = new HashSet<TblWebFormAccess>();
        }

        public int Id { get; set; }
        public string Fname { get; set; }
        public string Ename { get; set; }
        public int Type { get; set; }
        public int? Priority { get; set; }
        public bool? IsBold { get; set; }
        public string Color { get; set; }

        public virtual ICollection<TblWebFormAccess> TblWebFormAccessAccesses { get; set; }
        public virtual ICollection<TblWebFormAccess> TblWebFormAccessForms { get; set; }
    }
}
