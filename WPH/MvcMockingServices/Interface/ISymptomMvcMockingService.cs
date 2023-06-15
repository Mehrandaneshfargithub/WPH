using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Symptom;

namespace WPH.MvcMockingServices.Interface
{
    public interface ISymptomMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        bool CheckRepeatedSymptomName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "");
        Guid AddNewSymptom(SymptomViewModel newsymptom);
        Guid UpdateSymptom(SymptomViewModel symptom);
        OperationStatus RemoveSymptom(Guid symptomId);
        SymptomViewModel GetSymptom(Guid symptomId);
        IEnumerable<SymptomViewModel> GetAllSymptom(Guid clinicSectionId);
        IEnumerable<SymptomViewModel> GetAllSymptomForDisease(Guid clinicSectionId, Guid diseaseId, bool all);
        IEnumerable<SymptomViewModel> GetAllSymptomJustNameAndGuid(Guid clinicSectionId);
       

    }
}
