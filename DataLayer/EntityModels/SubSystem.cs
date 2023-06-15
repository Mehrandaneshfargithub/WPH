using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class SubSystem
    {
        public SubSystem()
        {
            Children = new HashSet<SubSystem>();
            SubSystemAccesses = new HashSet<SubSystemAccess>();
            SubSystemSections = new HashSet<SubSystemSection>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public int? Parent { get; set; }
        public string Link { get; set; }
        public string ShowName { get; set; }
        public bool? Active { get; set; }
        public string Icon { get; set; }
        public int? ParentRelationId { get; set; }

        public virtual SubSystem ParentRelation { get; set; }
        public virtual ICollection<SubSystem> Children { get; set; }

        public virtual ICollection<SubSystemAccess> SubSystemAccesses { get; set; }
        public virtual ICollection<SubSystemSection> SubSystemSections { get; set; }
    }
}
