using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class BaseInfoSectionType
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public Guid? BaseInfoTypeId { get; set; }
        public int? SectionTypeId { get; set; }

        public virtual BaseInfoType BaseInfoType { get; set; }
        public virtual BaseInfoGeneral SectionType { get; set; }
    }
}
