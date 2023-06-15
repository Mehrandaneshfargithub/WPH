using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.SectionManagement
{
    public class SectionsNameViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Priority { get; set; }
        public string Description { get; set; }
    }
}
