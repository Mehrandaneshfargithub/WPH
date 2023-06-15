using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class LanguageExpRepository : Repository<LanguageExpression>, ILanguageExpRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public LanguageExpRepository(WASContext context)
            : base(context)
        {
        }

        public int GetLanguageId(string Language)
        {
            return _context.Languages.SingleOrDefault(x => x.LanguageName == Language).Id;
        }

        public IEnumerable<LanguageExpression> GetAllExps(int langId)
        {

            IEnumerable<LanguageExpression> SubSystems = _context.LanguageExpressions.Include(x => x.Expression)
                                                                      .Where(t => t.LanguageId == langId).OrderBy(x=>x.Expression.Id);

            return SubSystems;
        }
    }
}
