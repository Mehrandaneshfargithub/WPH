using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface ILanguageExpRepository : IRepository<LanguageExpression>
    {
        int GetLanguageId(string Language);
        IEnumerable<LanguageExpression> GetAllExps(int langId);
    }
}
