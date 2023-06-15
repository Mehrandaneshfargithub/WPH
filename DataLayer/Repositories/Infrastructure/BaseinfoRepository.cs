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
    public class BaseinfoRepository : Repository<BaseInfo>, IBaseinfoRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public BaseinfoRepository(WASContext context)
            : base(context)
        {
        }

        public BaseInfoType GetBaseInfoType(Expression<Func<BaseInfoType, bool>> predicate)
        {
            return _context.BaseInfoTypes.AsNoTracking().SingleOrDefault(predicate);
        }
        public IEnumerable<BaseInfoType> GetAllBaseInfoTypes()
        {
            return _context.BaseInfoTypes.AsNoTracking().ToList();
        }

        public Guid GetBaseInfoTypeIdByName(string baseInfoTypeName)
        {
            return _context.BaseInfoTypes.AsNoTracking().FirstOrDefault(x => x.Ename == baseInfoTypeName).Guid;
        }

        public IEnumerable<BaseInfoType> GetAllBaseInfoTypesForSpecificSection(int sectionTypeId)
        {
            return _context.BaseInfoSectionTypes.AsNoTracking().Where(x => x.SectionTypeId == sectionTypeId).Select(x => x.BaseInfoType);
        }

        public Guid? GetIdByNameAndType(string name, string type, Guid? clinicSectionId)
        {
            //var typeId = _context.BaseInfoTypes.AsNoTracking().FirstOrDefault(p => p.Ename == type)?.Guid;
            return _context.BaseInfos.AsNoTracking()
                .Include(p => p.Type)
                .Where(p => p.Type.Ename == type && p.Name == name && /*p.TypeId == typeId && */p.ClinicSectionId == clinicSectionId)
                .FirstOrDefault()?.Guid;

        }

        public void RemoveBaseInfoType(BaseInfoType baseInfoType)
        {
            _context.BaseInfoTypes.Remove(baseInfoType);
        }

        public void AddBaseInfoType(BaseInfoType baseInfoType)
        {
            _context.BaseInfoTypes.Add(baseInfoType);
        }

        public void UpdateBaseInfoType(BaseInfoType baseInfoType)
        {
            _context.BaseInfoTypes.Update(baseInfoType);
        }

        public IEnumerable<BaseInfoSectionType> GetBaseInfoSectionTypeByBaseInfoTypeId(Guid baseInfoTypeId)
        {
            return _context.BaseInfoSectionTypes.AsNoTracking()
                .Where(p => p.BaseInfoTypeId == baseInfoTypeId);
        }

        public void RemoveRangeBaseInfoSectionTypes(List<BaseInfoSectionType> sectionTypes)
        {
            _context.BaseInfoSectionTypes.RemoveRange(sectionTypes);
        }

        public void AddRangeBaseInfoSectionTypes(List<BaseInfoSectionType> sectionTypes)
        {
            _context.BaseInfoSectionTypes.AddRange(sectionTypes);
        }

        public BaseInfoType GetTypeWithBaseInfoByNameAndType(string name, string typeName, Guid clinicSectionId)
        {
            return _context.BaseInfoTypes.AsNoTracking()
                .Include(p => p.BaseInfos)
                .Where(p => p.Ename == typeName)
                .Select(p => new BaseInfoType
                {
                    Guid = p.Guid,
                    BaseInfos = (ICollection<BaseInfo>)p.BaseInfos.Where(x => x.ClinicSectionId == clinicSectionId && x.Name == name).Select(s => new BaseInfo
                    {
                        Guid = s.Guid
                    })
                })
                .FirstOrDefault()
                ;
        }
    }
}
