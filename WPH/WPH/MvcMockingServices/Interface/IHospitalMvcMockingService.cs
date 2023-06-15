using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Hospital;

namespace WPH.MvcMockingServices.Interface
{
    public interface IHospitalMvcMockingService
    {
        OperationStatus RemoveHospital(Guid Hospitalid);
        Guid AddNewHospital(HospitalViewModel Hospital);
        Guid UpdateHospital(HospitalViewModel Hosp);
        //HospitalViewModel GetHospital(Guid HospitalId);
        bool CheckRepeatedHospitalName(string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<HospitalViewModel> GetAllHospitals();
        HospitalViewModel GetHospital(Guid HospitalId);
        void GetModalsViewBags(dynamic viewBag);
    }
}
