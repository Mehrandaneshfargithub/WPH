using System;
using System.Collections.Generic;

#nullable disable

namespace DMSDataLayer.EntityModels
{
    public partial class TblBaseInfoType
    {
        public TblBaseInfoType()
        {
            TblBaseInfos = new HashSet<TblBaseInfo>();
        }

        public int Id { get; set; }
        public string Ename { get; set; }
        public string Fname { get; set; }

        public virtual ICollection<TblBaseInfo> TblBaseInfos { get; set; }
    }
}
