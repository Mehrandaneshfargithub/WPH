using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Doctor CheckRepeatedDoctorNameAndPhone(Guid clinicSectionId, string name, string phoneNumber, string nameHolder = "", string phoneNumberHolder = "");
        IEnumerable<Doctor> GetAllDoctors(bool forGrid, Guid? clinicSectionId);
        Doctor CheckRepeatedDoctorNameAndSpeciallity(Guid clinicSectionId, string name, Guid? specialityId, string nameHolder = "", Guid? specialityIdHolder = null);
        Doctor GetDoctorById(Guid doctorId);
        Doctor GetDoctorByName(Guid clinicSectionId, string name);
        IEnumerable<Doctor> GetDoctorsBasedOnUserSection(List<Guid> sections);
        IEnumerable<Doctor> GetAllDoctorsForFilter(Guid clinicSectionId);
    }
}
