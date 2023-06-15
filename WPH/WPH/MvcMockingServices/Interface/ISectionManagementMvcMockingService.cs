using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.SectionManagement;
using WPH.Helper;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISectionManagementMvcMockingService
    {
        OperationStatus RemoveSection(int sectionId);
        OperationStatus RemoveClinicSection(Guid clinicSectionId);
        List<SectionsNameViewModel> GetAllSections();
        List<SectionsNameViewModel> GetAllClinicSections();
        SectionsNameViewModel GetSection(int id);
        List<ClinicSectionNamesViewModel> GetAllClinicSectionsBySectionTypeId(int sectionTypeId);
        List<ClinicSectionNamesViewModel> GetClinicSectionParents();
        ClinicSectionNamesViewModel GetClinicSection(Guid id);
        string AddNewSection(SectionsNameViewModel section);
        string UpdateSection(SectionsNameViewModel section);
        string AddNewClinicSection(ClinicSectionNamesViewModel clinicSection);
        string UpdateClinicSection(ClinicSectionNamesViewModel clinicSection);
        List<SubsystemViewModel> GetSubsystemParents();
        SubsystemViewModel GetSubsystem(int id);
        List<SubsystemAccessViewModel> GetSubsystemAccess(int subSystemId);
        void AddSubsystemAccess(int subSystemId, List<SubsystemAccessViewModel> accessList, List<SubsystemAccessViewModel> sectionTypes);
    }
}
