using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Service;

namespace WPH.MvcMockingServices.Interface
{
    public interface IServiceMvcMockingService
    { 
        IEnumerable<ServiceViewModel> GetAllService(Guid clinicSectionId);
        OperationStatus RemoveService(Guid Serviceid);
        string AddNewService(ServiceViewModel Service);
        string UpdateService(ServiceViewModel Service);
        ServiceViewModel GetService(Guid ServiceId);
        void GetModalsViewBags(dynamic viewBag);
        bool CheckRepeatedServiceName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "");
        IEnumerable<ServiceViewModel> GetAllSpeceficServices(string serviceType, Guid clinicSectionId);
        IEnumerable<ServiceViewModel> GetAllServicesExceptOperation(Guid clinicSectionId);
        ServiceViewModel GetServiceType(string typeName);
        List<ServiceReportViewModel> GetAllOperationsForReport(Guid clinicSectionId);
    }
}
