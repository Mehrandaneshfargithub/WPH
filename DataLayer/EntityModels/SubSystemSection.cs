using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class SubSystemSection
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int SubSystemId { get; set; }
        public int SectionTypeId { get; set; }

        public virtual BaseInfoGeneral SectionType { get; set; }
        public virtual SubSystem SubSystem { get; set; }
    }
}
