using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories.Infrastructure
{
    public class BaseinfoGeneralRepository : Repository<BaseInfoGeneral>, IBaseinfoGeneralRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public BaseinfoGeneralRepository(WASContext context)
            : base(context)
        {
        }

        public BaseInfoGeneralType GetBaseInfoGeneralType(Expression<Func<BaseInfoGeneralType, bool>> predicate)
        {
            return _context.BaseInfoGeneralTypes.AsNoTracking().SingleOrDefault(predicate);
        }

        public int? GetIdByNameAndType(string name, string type)
        {
            var typeId = _context.BaseInfoGeneralTypes.AsNoTracking().FirstOrDefault(p => p.Ename == type)?.Id;
            return _context.BaseInfoGenerals.AsNoTracking().FirstOrDefault(p => p.Name == name && p.TypeId == typeId)?.Id;

        }

        public BaseInfoGeneral GetByIdAndType(int id, string type)
        {
            var typeId = _context.BaseInfoGeneralTypes.AsNoTracking().FirstOrDefault(p => p.Ename == type)?.Id;
            return _context.BaseInfoGenerals.AsNoTracking().FirstOrDefault(p => p.Id == id && p.TypeId == typeId);
        }

        public IEnumerable<BaseInfoGeneral> GetAllNamesByType(string type) 
        {
            return _context.BaseInfoGenerals.AsNoTracking()
                .Include(p => p.Type)
                .Where(p => p.Type.Ename == type)
                .Select(p => new BaseInfoGeneral
                {
                    Id = p.Id,
                    Name = p.Name,
                    Priority = p.Priority,
                    Description = p.Description
                });
        }

        public bool CheckNameExists(string name, string type, Expression<Func<BaseInfoGeneral, bool>> predicate = null)
        {
            IQueryable<BaseInfoGeneral> result = _context.BaseInfoGenerals.AsNoTracking()
                .Include(p => p.Type)
                .Where(p => p.Name == name && p.Type.Ename == type);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Any();
        }

        public IEnumerable<BaseInfoGeneral> GetSectionType(Guid baseInfoTypeId)
        {
            return _context.BaseInfoGenerals.AsNoTracking()
                .Include(p => p.Type)
                .Include(p => p.BaseInfoSectionTypes)
                .Where(p => p.Type.Ename == "SectionType")
                .Select(p => new BaseInfoGeneral
                {
                    Id = p.Id,
                    Name = p.Name,
                    BaseInfoSectionTypes = (ICollection<BaseInfoSectionType>)p.BaseInfoSectionTypes.Where(w => w.BaseInfoTypeId == baseInfoTypeId).Select(x => new BaseInfoSectionType
                    {
                        BaseInfoTypeId = x.BaseInfoTypeId,
                    })
                });
        }

        public IEnumerable<BaseInfoGeneral> GetSubsystemSectioType(int subSystemId)
        {
            return _context.BaseInfoGenerals.AsNoTracking()
                .Include(p => p.Type)
                .Include(p => p.SubSystemSections)
                .Where(p => p.Type.Ename == "SectionType")
                .Select(p => new BaseInfoGeneral
                {
                    Id = p.Id,
                    Name = p.Name,
                    SubSystemSections = (ICollection<SubSystemSection>)p.SubSystemSections.Where(w => w.SubSystemId == subSystemId).Select(x => new SubSystemSection
                    {
                        SectionTypeId = x.SectionTypeId,
                    })
                });
        }

        public IEnumerable<BaseInfoGeneral> GetByTypeAndNames(string type, IEnumerable<string> names)
        {
            return _context.BaseInfoGenerals.AsNoTracking()
                .Where(p => names.Contains(p.Name) && p.Type.Ename == type)
                .Select(p => new BaseInfoGeneral
                {
                    Id = p.Id,
                    Name = p.Name,
                });
        }

        public string GetNameById(int id)
        {
            return _context.BaseInfoGenerals.AsNoTracking()
                .SingleOrDefault(p => p.Id == id)?.Name;
        }
    }
}
