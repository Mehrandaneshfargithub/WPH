using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IBaseinfoGeneralRepository : IRepository<BaseInfoGeneral>
    {
        BaseInfoGeneralType GetBaseInfoGeneralType(Expression<Func<BaseInfoGeneralType, bool>> predicate);
        int? GetIdByNameAndType(string name, string type);
        BaseInfoGeneral GetByIdAndType(int id, string type);
        IEnumerable<BaseInfoGeneral> GetAllNamesByType(string type);
        bool CheckNameExists(string name, string type, Expression<Func<BaseInfoGeneral, bool>> predicate = null);
        IEnumerable<BaseInfoGeneral> GetSectionType(Guid baseInfoTypeId);
        IEnumerable<BaseInfoGeneral> GetSubsystemSectioType(int subSystemId);
        IEnumerable<BaseInfoGeneral> GetByTypeAndNames(string type, IEnumerable<string> names);
        string GetNameById(int id);
    }
}
