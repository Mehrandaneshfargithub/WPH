using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;

namespace DataLayer.Repositories.Infrastructure
{
    public class ExpressionRepository : Repository<AllExpression>, IExpressionRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ExpressionRepository(WASContext context)
            : base(context)
        {
        }

    }
}
