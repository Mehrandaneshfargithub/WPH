using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class BaseInfoGeneralType
    {
        public BaseInfoGeneralType()
        {
            BaseInfoGenerals = new HashSet<BaseInfoGeneral>();
        }

        public int Id { get; set; }
        public string Ename { get; set; }
        public string Fname { get; set; }

        public virtual ICollection<BaseInfoGeneral> BaseInfoGenerals { get; set; }
    }
}
