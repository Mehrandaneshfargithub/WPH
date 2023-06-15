using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.AnalysisResultTemplate;

namespace WPH.MvcMockingServices.Interface
{
    public interface IAnalysisResultTemplateMvcMockingService
    {
        void GetModalsViewBags(dynamic viewBag);
        Guid AddNewAnalysisResultTemplate(AnalysisResultTemplateViewModel newAnalysisResultTemplate);
        Guid UpdateAnalysisResultTemplate(AnalysisResultTemplateViewModel AnalysisResultTemplate);
        OperationStatus RemoveAnalysisResultTemplate(Guid AnalysisResultTemplateId);
        AnalysisResultTemplateViewModel GetAnalysisResultTemplate(Guid AnalysisResultTemplateId);
        IEnumerable<AnalysisResultTemplateViewModel> GetAllAnalysisResultTemplate(Guid clinicSectionId);
        IEnumerable<AnalysisResultTemplateViewModel> GetAllAnalysisResultTemplateByUserId(Guid userId);
    }
}
