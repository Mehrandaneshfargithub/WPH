using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.CustomDataModels.ClinicSectionChoosenValue;

namespace WPH.MvcMockingServices.Interface
{
    public interface IClinicSectionChoosenValueMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        void RemoveClinicSectionChoosenValue(Guid ClinicSectionChoosenValueId);
        ClinicSectionChoosenValueViewModel GetClinicSectionChoosenValueById(Guid id);
        IEnumerable<ClinicSectionChoosenValueViewModel> GetAllClinicSectionChoosenValues(Guid clinicSectionId);
        IEnumerable<ClinicSectionChoosenValueViewModel> GetAllFillBySecretaryClinicSectionChoosenValues(Guid clinicSectionId);
        IEnumerable<ClinicSectionChoosenValueViewModel> GetAllNumericClinicSectionChoosenValues(Guid clinicSectionId);
        void AddClinicSectionChoosenValues(IEnumerable<ClinicSectionChoosenValueViewModel> clinicSectionChoosenValueDto, Guid clinicSectionId);
        void UpdateClinicSectionChoosenValue(ClinicSectionChoosenValueViewModel ClinicSectionChoosenValue);
        void RemoveClinicSectionChoosenValueRange(List<ClinicSectionChoosenValueViewModel> mostRemoved);
        void AddClinicSectionChoosenValue(ClinicSectionChoosenValueViewModel today);


    }
}
