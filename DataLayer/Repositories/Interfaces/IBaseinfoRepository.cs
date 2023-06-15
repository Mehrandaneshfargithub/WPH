using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IBaseinfoRepository : IRepository<BaseInfo>
    {
        BaseInfoType GetBaseInfoType(Expression<Func<BaseInfoType, bool>> predicate);
        IEnumerable<BaseInfoType> GetAllBaseInfoTypes();
        Guid GetBaseInfoTypeIdByName(string baseInfoTypeName);
        IEnumerable<BaseInfoType> GetAllBaseInfoTypesForSpecificSection(int sectionTypeId);
        Guid? GetIdByNameAndType(string name, string type, Guid? clinicSectionId);
        void RemoveBaseInfoType(BaseInfoType baseInfoType);
        void AddBaseInfoType(BaseInfoType baseInfoType);
        void UpdateBaseInfoType(BaseInfoType baseInfoType);
        IEnumerable<BaseInfoSectionType> GetBaseInfoSectionTypeByBaseInfoTypeId(Guid baseInfoTypeId);
        void RemoveRangeBaseInfoSectionTypes(List<BaseInfoSectionType> sectionTypes);
        void AddRangeBaseInfoSectionTypes(List<BaseInfoSectionType> sectionTypes);
        BaseInfoType GetTypeWithBaseInfoByNameAndType(string name, string typeName, Guid clinicSectionId);
    }
}
