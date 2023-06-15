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
    public class ChildRepository : Repository<Child>, IChildRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ChildRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Child> GetAllChild(Guid clinicSectionId)
        {
            return Context.Children.AsNoTracking()
                .Include(p => p.User).ThenInclude(p => p.Gender)
                .Where(p => p.User.ClinicSectionId == clinicSectionId)
                .OrderByDescending(o => o.BirthDate)
                .Select(s => new Child
                {
                    Guid = s.Guid,
                    Weight = s.Weight,
                    BirthDate = s.BirthDate,
                    User = new User
                    {
                        Name = s.User.Name,
                        Gender = new BaseInfoGeneral
                        {
                            Name = s.User.Gender.Name
                        }
                    }

                });
        }

        public IEnumerable<Child> GetAllUnknownChildren(Guid clinicSectionId)
        {
            return Context.Children.AsNoTracking()
                .Include(p => p.User)
                .Where(p => p.ReceptionId == null && p.User.ClinicSectionId == clinicSectionId)
                .OrderByDescending(o => o.BirthDate)
                .Select(s => new Child
                {
                    Guid = s.Guid,
                    User = new User
                    {
                        Name = s.User.Name
                    }
                });
        }

        public IEnumerable<Child> GetAllHospitalPatientChildren(Guid receptionId)
        {
            return Context.Children.AsNoTracking()
                .Include(p => p.User).ThenInclude(p => p.Gender)
                .Include(p => p.Doctor.User)
                .Include(p => p.Room.ClinicSection)
                .Where(p => p.ReceptionId == receptionId)
                .OrderByDescending(o => o.BirthDate)
                .Select(s => new Child
                {
                    Guid = s.Guid,
                    Weight = s.Weight,
                    BirthDate = s.BirthDate,
                    ReceptionDate = s.ReceptionDate,
                    User = new User
                    {
                        Name = s.User.Name,
                        Gender = new BaseInfoGeneral
                        {
                            Name = s.User.Gender.Name
                        }
                    },
                    Room = new Room
                    {
                        Name = s.Room.Name,
                        ClinicSection = new ClinicSection
                        {
                            Name = s.Room.ClinicSection.Name
                        }
                    },
                    Doctor = new Doctor
                    {
                        User = new User
                        {
                            Name = s.Doctor.User.Name
                        }
                    }
                });
        }


        public IEnumerable<Child> GetDetailChildReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<Child, bool>> predicate = null)
        {
            IQueryable<Child> result = _context.Children.AsNoTracking()
                 .Include(p => p.User).ThenInclude(p => p.Gender)
                 .Include(p => p.Status)
                 .Where(p => p.BirthDate >= fromDate && p.BirthDate <= toDate &&
                        clinicSectionId.Contains(p.User.ClinicSectionId ?? Guid.Empty))
                 ;

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(s => new Child
            {
                VitalActivities = s.VitalActivities,
                CongenitalAnomalies = s.CongenitalAnomalies,
                OperationOrder = s.OperationOrder,
                Weight = s.Weight,
                BirthDate = s.BirthDate,
                Status = new BaseInfoGeneral
                {
                    Name = s.Status.Name
                },
                User = new User
                {
                    Name = s.User.Name,
                    Gender = new BaseInfoGeneral
                    {
                        Name = s.User.Gender.Name
                    }
                }
            });
        }

        public Child GetChildWithUser(Guid childId)
        {
            return Context.Children.AsNoTracking()
                .Include(p => p.User)
                .SingleOrDefault(p => p.Guid == childId);
        }
    }
}
