using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        IEnumerable<Service> GetAllService(Expression<Func<Service, bool>> predicate = null);
        IEnumerable<Service> GetAllSpeceficServices(string serviceType, Guid clinicSectionId);
        IEnumerable<Service> GetAllServicesExceptOperation(Guid clinicSectionId);
        Service GetServiceByName(Guid? clinicSectionId, string serviceName);
        IEnumerable<Service> GetAllOperationsForReport(Guid clinicSectionId);
        Service GetService(Guid serviceId);
        bool CheckServiceExist(Expression<Func<Service, bool>> predicate);
    }
}
