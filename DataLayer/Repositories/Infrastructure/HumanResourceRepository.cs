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
    public class HumanResourceRepository : Repository<HumanResource>, IHumanResourceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public HumanResourceRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<HumanResource> GetAllHuman(List<Guid> sections, Expression<Func<HumanResource, bool>> predicate = null)
        {
            IQueryable<HumanResource> result = Context.HumanResources.AsNoTracking()
                                                .Include(x => x.Gu)
                                                .Include(x => x.RoleType)
                                                .Include(x => x.Currency);
            if (predicate != null)
                result = result.Where(predicate);

            return result.Where(p => sections.Contains(p.Gu.ClinicSectionId ?? Guid.Empty));
        }


        public IEnumerable<HumanResource> GetAllTreatmentStaff(List<Guid> sections, Expression<Func<HumanResource, bool>> predicate = null)
        {
            IQueryable<HumanResource> result = Context.HumanResources.AsNoTracking()
                                                .Include(x => x.Gu);
            if (predicate != null)
                result = result.Where(predicate);

            return result.Where(p => sections.Contains(p.Gu.ClinicSectionId ?? Guid.Empty))
                .Select(s => new HumanResource
                {
                    Guid = s.Guid,
                    Gu = new User
                    {
                        Name = s.Gu.Name
                    }
                });
        }
        //public IEnumerable<HumanResource> GetAllHumanWithPeriods(DateTime dateFrom, DateTime dateTo)
        //{
        //    return Context.HumanResources.AsNoTracking()
        //        .Include(x => x.Gu)
        //        .Include(x => x.RoleType)
        //        .Include(x => x.Currency)
        //        .Include(x => x.HumanResourceSalaries)
        //        .Where(a => a.HumanResourceSalaries.All(x => x.BeginDate >= dateFrom && x.EndDate <= dateTo));
        //}


        public HumanResource GetHumanById(Guid? humanId)
        {
            return _context.HumanResources.AsNoTracking()
                .Include(x => x.Gu.Doctor.Speciality)
                .Include(x => x.RoleType)
                .Include(x => x.Currency)
                .SingleOrDefault(x => x.Guid == humanId);
        }


        public HumanResource GetHumanByName(string humanName)
        {
            return _context.HumanResources.AsNoTracking()
                .Include(x => x.Gu)
                .Include(x => x.RoleType)
                .Include(x => x.Currency).SingleOrDefault(x => x.Gu.Name == humanName);
        }

        public HumanResource GetById(Guid? humanId)
        {
            return _context.HumanResources.AsNoTracking()
                    .SingleOrDefault(x => x.Guid == humanId);
        }

    }
}
