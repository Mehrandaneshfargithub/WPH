using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IClinicSection_UserRepository : IRepository<ClinicSectionUser>
    {
        //ClinicSection_User ClinicSection_User_BasedClinicIdAndUserName(Guid ClinicId, string userName, string currentUserName = "");
        //ClinicSection_User ClinicSection_User_BasedClinicIdAndNameAndPhoneNumber(Guid ClinicId, string Name,  string number, string currentName = "", string currentnumber = "");
        IEnumerable<ClinicSection> GetClinicSectionsForUser(Guid userId, string type, Guid? parentId = null);
        IEnumerable<ClinicSection> GetAllUserClinicSections(Guid userId, Guid clinicSectionId);
        IEnumerable<ClinicSection> GetAllClinicSectionsForUserWithParent(Guid userId, Guid parentId);
        IEnumerable<ClinicSection> GetAllClinicSectionParentsForUser(Guid userId, Guid clinicId);
    }
}
