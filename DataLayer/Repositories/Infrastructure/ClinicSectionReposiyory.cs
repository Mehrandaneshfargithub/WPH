using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class ClinicSectionReposiyory : Repository<ClinicSection>, IClinicSectionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ClinicSectionReposiyory(WASContext context)
            : base(context)
        {
        }

        public List<ClinicSection> GetClinicSectionsByIds(List<Guid> clinicSectionIds)
        {
            return _context.ClinicSections.AsNoTracking().Where(x => clinicSectionIds.Contains(x.Guid)).ToList();
        }

        public ClinicSection GetWithSectionName(Guid clinicSectionId)
        {
            return _context.ClinicSections.AsNoTracking().Include(x => x.SectionType).SingleOrDefault(x => x.Guid == clinicSectionId);
        }

        public IEnumerable<DashboardPoco> GetAllDashboardDatas(Guid clinicSectionId)
        {
            try
            {
                return _context.Receptions.Where(x => x.ClinicSectionId == clinicSectionId).Select(x => new DashboardPoco
                {
                    Date = x.ReceptionDate,
                    DateOfBirth = x.ReserveDetail.Patient.DateOfBirth,
                });

            }
            catch (Exception e) { throw e; }
        }

        public void UpdateClinicSectionName(Guid clinicSectionId, string name)
        {
            ClinicSection cs = _context.ClinicSections.SingleOrDefault(x => x.Guid == clinicSectionId);
            cs.Name = name;
            _context.SaveChanges();
        }

        public IEnumerable<ClinicSection> GetAllClinicSectionWithType(Guid clinicId)
        {
            return _context.ClinicSections.AsNoTracking()
                .Select(a => new ClinicSection
                {
                    Name = a.Name,
                    ParentId = a.ParentId,
                    Guid = a.Guid,
                    SectionTypeId = a.SectionTypeId
                }).Where(x => x.ParentId == clinicId && x.SectionTypeId == null);

        }

        public IEnumerable<ClinicSection> GetAllParentClinicSections()
        {

            Guid main = _context.ClinicSections.AsNoTracking().SingleOrDefault(x => x.ParentId == null).Guid;

            return _context.ClinicSections.AsNoTracking().Where(x => x.ParentId == main)
                .Select(a => new ClinicSection
                {
                    Name = a.Name,
                    Guid = a.Guid,
                });

        }

        public IEnumerable<Guid> GetClinicSectionChilds(List<Guid> clinicSectionIds, Guid? UserId)
        {

            if (UserId == null)
            {
                List<Guid> All = new List<Guid>();
                All.AddRange(_context.ClinicSections.AsNoTracking().Where(x => clinicSectionIds.Contains(x.ParentId ?? Guid.Empty)).Select(a => a.Guid));
                All.AddRange(_context.ClinicSections.AsNoTracking().Where(x => All.Contains(x.ParentId ?? Guid.Empty)).Select(a => a.Guid));
                return All;

            }
            else
            {
                List<Guid> All = new List<Guid>();
                All.AddRange(_context.ClinicSections.AsNoTracking().Where(x => clinicSectionIds.Contains(x.ParentId ?? Guid.Empty)).Select(a => a.Guid));
                All.AddRange(_context.ClinicSections.AsNoTracking().Where(x => All.Contains(x.ParentId ?? Guid.Empty)).Select(a => a.Guid));
                //IEnumerable<Guid> dd = _context.ClinicSections.AsNoTracking()
                //.Join(_context.ClinicSectionUsers.Where(a => a.UserId == UserId),
                //        clinicsection => clinicsection.Guid,
                //        clinicSectionUsers => clinicSectionUsers.ClinicSectionId,
                //        (clinicSectionUsers, clinicsection) => clinicSectionUsers)
                //        .Where(x => All.Contains(x.Guid)).Select(a => a.Guid);
                IEnumerable<Guid> dd = _context.ClinicSectionUsers.AsNoTracking().Where(a => a.UserId == UserId && All.Contains(a.ClinicSectionId)).Select(a => a.ClinicSectionId);

                return dd;
            }
        }
        public bool ClinicSectionHasChild(Guid clinicSectionId, Guid UserId)
        {
            List<Guid> All = new List<Guid>();
            All.AddRange(_context.ClinicSections.AsNoTracking().Where(x => x.ParentId == clinicSectionId).Select(a => a.Guid));

            return _context.ClinicSectionUsers.AsNoTracking().Where(a => a.UserId == UserId && All.Contains(a.ClinicSectionId)).Any();

        }
        public IEnumerable<ClinicSection> GetAllMainClinicSectionsExceptOne(Guid clinicSectionId)
        {
            return _context.ClinicSections.AsNoTracking()
                .Where(x => x.Guid != clinicSectionId && x.ClinicSectionTypeId == null && x.ParentId != null)
                .Select(x => new ClinicSection
                {
                    Guid = x.Guid,
                    Name = x.Name
                });

        }

        public IEnumerable<ClinicSection> GetAllClinicSectionsChild(Guid clinicSectionId, Guid userId)
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection)
                .Where(x => x.UserId == userId && x.ClinicSection.ParentId == clinicSectionId)
                .Select(x => x.ClinicSection);

        }

        public IEnumerable<ClinicSection> GetAllClinicSectionsChildForTransferSource(Guid clinicSectionId, Guid userId)
        {
            return _context.ClinicSectionUsers.AsNoTracking()
                .Include(x => x.ClinicSection)
                .Where(x => x.UserId == userId && x.ClinicSection.ParentId == clinicSectionId)
                .Select(x => x.ClinicSection).Union(_context.ClinicSections.Where(a => a.ParentId == clinicSectionId && a.SectionTypeId == null));


        }


        public IEnumerable<ClinicSection> GetAllClinicSectionsBySectionTypeId(Expression<Func<ClinicSection, bool>> predicate = null)
        {
            IQueryable<ClinicSection> result = _context.ClinicSections.AsNoTracking()
               .Include(x => x.Parent)
               .Include(x => x.SectionType)
               .Include(x => x.ClinicSectionType)
               ;
            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new ClinicSection
            {
                Guid = p.Guid,
                Name = p.Name,
                SectionTypeId = p.SectionTypeId,
                Priority = p.Priority,
                SectionType = new BaseInfoGeneral
                {
                    Name = p.SectionType.Name
                },
                ClinicSectionTypeId = p.ClinicSectionTypeId,
                ClinicSectionType = new BaseInfoGeneral
                {
                    Name = p.ClinicSectionType.Name
                },
                ParentId = p.ParentId,
                Parent = new ClinicSection
                {
                    Name = p.Parent.Name
                }
            });
        }

        public IEnumerable<ClinicSection> GetClinicSectionParents()
        {
            return _context.ClinicSections.AsNoTracking()
               .Where(p => p.ParentId == null || p.ClinicSectionTypeId == null)
               .Select(p => new ClinicSection
               {
                   Guid = p.Guid,
                   Name = p.Name,
               });
        }

        public bool CheckNameExists(string name, int? clinicSectionTypeId, Expression<Func<ClinicSection, bool>> predicate = null)
        {
            IQueryable<ClinicSection> result = _context.ClinicSections.AsNoTracking()
                .Where(p => p.Name == name && p.ClinicSectionTypeId == clinicSectionTypeId);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public bool CheckClinicSectionIsParent(Guid sourceClinicSectionId, Guid destinationClinicSectionId)
        {
            var clinicSections = _context.ClinicSections.AsNoTracking().Where(p => p.Guid == sourceClinicSectionId || p.Guid == destinationClinicSectionId);
            var destinationClinicSection = clinicSections.FirstOrDefault(p => p.Guid == destinationClinicSectionId);
            var sourceClinicSection = clinicSections.FirstOrDefault(p => p.Guid == sourceClinicSectionId);
            var is_parent = sourceClinicSection.ClinicSectionTypeId == null && destinationClinicSection.ClinicSectionTypeId == null && sourceClinicSection.SectionTypeId != null && destinationClinicSection.SectionTypeId != null;

            return is_parent;
        }

        public IEnumerable<ClinicSection> GetAllClinicSectionsWithChilds(Guid clinicId)
        {
            return _context.ClinicSections.AsNoTracking()
               .Where(p => p.ClinicId == clinicId)
               .Select(p => new ClinicSection
               {
                   Guid = p.Guid,
                   Name = p.Name,
                   ParentId = p.ParentId,
                   ClinicSectionTypeId = p.ClinicSectionTypeId
               });
        }

        public IEnumerable<ClinicSection> GetAllAccessedUserClinicSectionWithChilds(Guid userId)
        {
            IQueryable<Guid> all = _context.ClinicSectionUsers.AsNoTracking()
                .Where(p => p.UserId == userId).Select(a => a.ClinicSectionId);

            var result = _context.ClinicSections.AsNoTracking().Where(a=>all.Contains(a.ParentId.Value) || all.Contains(a.Guid))
                .Select(p => new ClinicSection
                {
                    Guid = p.Guid,
                    Name = p.Name,
                    ClinicSectionTypeId = p.ClinicSectionTypeId
                });

            return result;

        }
    }
}
