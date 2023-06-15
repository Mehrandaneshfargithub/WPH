using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.UserClinicSection;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.MvcMockingServices.Interface
{
    public interface IUserMvcMockingService
    {
        void saveCookie(string cookie, string cookieName, string cookieValue);
        string LoadLoginThemeCookie();
        Guid GetUserIdByUserName(Guid clinicSectionId, string UserName);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<UserClinicSectionViewModel> GetAllClinicsUsers(Guid clinicId);
        void SaveUserAccess(Guid userId, List<string> checkedIds, Guid clinicSectionId);
        void loginSettings(dynamic viewBag);
        void AddUserClinicSections(string itemList, Guid UserId, Guid clinicSectionId);
        bool GetSoftwareSetting(string setting);
        OperationStatus RemoveUser(Guid userId);
        UserInformationViewModel CheckUserExist(string userName, string password, Guid clinicId, int? userId);
        int CheckUserCount(string userName, string password, Guid clinicSectionId, int? userId);
        Guid CheckClinicExist(string code);
        string UpdateUser(UserInformationViewModel user, bool IsMD5 = false);
        void UserActivationStatus(Guid id);
        void loginTheme(dynamic viewBag);
        void RemoveUserClinicSection(Guid userClinicSectionId);
        IEnumerable<UserInformationViewModel> GetAllUsers(Guid clinicSectionId, Guid humanId/*, string UserAccessType, Guid clinicId*/);
        string AddNewUser(UserInformationViewModel newUser);
        void AddNewUserToClinic(UserInformationViewModel user);
        UserInformationViewModel GetUser(Guid userId);
        UserInformationViewModel GetUser(Guid clinicSectionId, string userName);
        bool CheckRepeatedUserName(string username, Guid clinicId, bool NewOrUpdate, string oldName = "");
        List<SubSystemsWithAccessViewModel> GetAllSubSystemsWithAccessNames(Guid userId, int sectionTypeId, Guid clinicSectionId, Guid parentUserId);
        void DeleteAllUserAccess(Guid userId, Guid clinicSectionId);
        void SaveAccess(Guid userId, int subSystemAccessId, Guid clinicSectionId);
        bool IsUserNameExist(string username, Guid clinicId, bool newOrUpdate, string currentUserName);
        bool IsNameExist(string name, Guid clinicId, bool newOrUpdate, string currentName);
        UserInformationViewModel GetUserWithAccess(Guid userId);
        IEnumerable<UserInformationViewModel> GetAllDoctors();
        UserInformationViewModel GetUserWithRole(Guid userId);
        UserInformationViewModel CheckAdminExist(string userName, string password);
        void EditCurrentUser(UserInformationViewModel user);
        bool PastPassIsCorrect(UserInformationViewModel user);
        IEnumerable<UserInformationViewModel> GetAllUsersExeptUserPortions(Guid clinicSectionId);
    }
}
