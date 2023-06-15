using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ServiceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Service> GetAllService(Expression<Func<Service, bool>> predicate = null)
        {
            IQueryable<Service> result = Context.Services.AsNoTracking()
                                        .Include(p => p.Type)
                                        .Include(p => p.OperationType);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new Service
            {
                Guid = p.Guid,
                Name = p.Name,
                Price = p.Price,
                DoctorWage = p.DoctorWage,
                Explanation = p.Explanation,
                OperationTypeId = p.OperationTypeId,
                Type = new BaseInfoGeneral
                {
                    Name = p.Type.Name
                },
                OperationType = new BaseInfo
                {
                    Name = p.OperationType.Name
                }
            });
        }

        public IEnumerable<Service> GetAllSpeceficServices(string serviceType, Guid clinicSectionId)
        {
            return Context.Services.AsNoTracking().
                Include(p => p.Type).Where(a => a.Type.Name == serviceType && a.ClinicSectionId == clinicSectionId);
        }
        public IEnumerable<Service> GetAllServicesExceptOperation(Guid clinicSectionId)
        {
            return Context.Services.AsNoTracking().
                Include(p => p.Type).Where(a => a.Type.Name != "Operation" && a.ClinicSectionId == clinicSectionId);
        }

        public Service GetServiceByName(Guid? clinicSectionId, string serviceName)
        {
            return Context.Services.AsNoTracking()
                .SingleOrDefault(p => p.ClinicSectionId == clinicSectionId && p.Name == serviceName);
        }

        public IEnumerable<Service> GetAllOperationsForReport(Guid clinicSectionId)
        {
            return Context.Services.AsNoTracking()
                .Include(p => p.Type)
                .Where(a => a.ClinicSectionId == clinicSectionId)
                .Select(p => new Service
                {
                    Name = p.Name,
                    Price = p.Price,
                    Type = new BaseInfoGeneral
                    {
                        Name = p.Type.Name
                    }
                });
        }

        public Service GetService(Guid serviceId)
        {
            return _context.Services.AsNoTracking()
                .Include(p => p.OperationType)
                .SingleOrDefault(p => p.Guid == serviceId);
        }

        public bool CheckServiceExist(Expression<Func<Service, bool>> predicate)
        {
            return _context.Services.AsNoTracking()
                .Where(predicate).Any();
        }
    }
}
