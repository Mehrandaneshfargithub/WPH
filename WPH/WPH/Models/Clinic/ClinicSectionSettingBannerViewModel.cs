using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.Models.Clinic
{
    public class ClinicSectionSettingBannerViewModel
    {
        public Guid ClinicSectionId { get; set; }
        public int SettingId { get; set; }
        public IFormFile Banner { get; set; }
    }
}
