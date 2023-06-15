using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class BaseInfoType
    {
        public BaseInfoType()
        {
            BaseInfoSectionTypes = new HashSet<BaseInfoSectionType>();
            BaseInfos = new HashSet<BaseInfo>();
        }

        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Ename { get; set; }
        public string Fname { get; set; }

        public virtual ICollection<BaseInfoSectionType> BaseInfoSectionTypes { get; set; }
        public virtual ICollection<BaseInfo> BaseInfos { get; set; }
    }
}
