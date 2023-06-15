using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class AnalysisResultTemplate
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSection ClinicSection { get; set; }
    }
}
