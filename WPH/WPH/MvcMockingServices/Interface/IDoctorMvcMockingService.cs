using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using WPH.Helper;
using WPH.Models.CustomDataModels.Doctor;
using WPH.Models.Doctor;

namespace WPH.MvcMockingServices.Interface
{
    public interface IDoctorMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<DoctorViewModel> GetAllDoctorsWithCombinedNameAndSpeciallity(bool forGrid, Guid clinicSectionId);
        List<DoctorViewModel> GetAllDoctor(bool forGrid, Guid? clinicSectionId = null);
        string AddNewDoctor(DoctorViewModel newDoctor);
        string UpdateDoctor(DoctorViewModel Doctor);
        OperationStatus RemoveDoctor(Guid DoctorId);
        DoctorViewModel GetDoctor(Guid DoctorId);
        bool CheckRepeatedDoctorNameAndPhone(string name, string phoneNumber, Guid clinicSectionId, bool newOrUpdate, string nameHolder = "", string phoneNumberHolder = "");
        bool CheckRepeatedNameAndSpeciallity(string name, Guid? specialityId, Guid clinicSectionId, bool newOrUpdate, string nameHolder = "", Guid? specialityIdHolder = null);
        List<DoctorViewModel> GetDoctorsBasedOnUserSection(List<Guid> sections);
        List<DoctorFilterViewModel> GetAllDoctorsForFilter(Guid clinicSectionId);
        string ConvertDoctorToUser(Guid doctorId);
        DoctorViewModel GetDoctorLogoAddress(Guid doctorId);
        string SaveDoctorReportLogo(Guid doctorId, IFormFile reportLogo, string rootPath);
        void RemoveDoctorReportLogo(Guid doctorId, string rootPath);
    }
}
