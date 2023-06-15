using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels;

namespace WPH.Areas.Admin.Models.LicenceKeyManagement
{
    public class LicenceKeyManagementViewModel : IndexViewModel
    {
        public int Index { get; set; }
        public int Id { get; set; }
        public string SerialKey { get; set; }
        public string ComputerSerial { get; set; }
        public string Date { get; set; }
    }
}
