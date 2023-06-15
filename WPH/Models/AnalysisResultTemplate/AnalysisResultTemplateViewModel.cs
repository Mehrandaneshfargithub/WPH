using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;
using WPH.Models.CustomDataModels.ClinicSection;

namespace WPH.Models.AnalysisResultTemplate
{
    public class AnalysisResultTemplateViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string NameHolder { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public Guid? ClinicSectionId { get; set; }

        public virtual ClinicSectionViewModel ClinicSection { get; set; }
    }
}
