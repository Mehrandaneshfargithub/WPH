using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using WPH.Areas.Admin.Models.SectionManagement;
using WPH.Helper;
using WPH.Models.Access;
using WPH.Models.CustomDataModels.UserManagment;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISubSystemMvcMockingService
    {
        List<SubSystemViewModel> GetMenuItems(Guid userId, int sectionTypeId, Guid clinicSectionId);
        List<SubSystemViewModel> GetAllSubSystemsForDev(int SectionTypeId);
        List<SubSystemViewModel> GetSubSystemByName(string SubSystemName);
        void DeleteAllUserAccess(Guid userId, Guid clinicSectionId);
        void SaveAccessForUser(Guid userId, List<string> subSystemAccessId, Guid clinicSectionId);
        IEnumerable<AllSubSystemsWithAccess> GetAllSubSystemsWithAccessNames(Guid userId, int SectionTypeId, Guid clinicSectionId, int LanguageId, Guid parentUserId);
        bool CheckUserAccess(string accessName, string subSystemName);
        List<AccessViewModel> GetUserSubSystemAccess(params string[] subSystemsName);
        List<SubsystemViewModel> GetAllSubsystemsWithParent();
        string AddNewSubsystem(SubsystemViewModel subsystem);
        string UpdateSubsystem(SubsystemViewModel subsystem);
        OperationStatus RemoveSubsystem(int subsystemId);
        void ChangeSubsystemActivation(int subsystemId);
    }
}
