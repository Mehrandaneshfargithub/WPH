using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Clinic
{
    public class ClinicSectionUserViewModel : IndexViewModel
    {
        public Guid Guid { get; set; }
        public int Index { get; set; }
        public int Id { get; set; }
        public System.Guid ClinicSectionId { get; set; }
        public System.Guid UserId { get; set; }

    }
}