using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisResultTemplateRepository : Repository<AnalysisResultTemplate>, IAnalysisResultTemplateRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisResultTemplateRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<AnalysisResultTemplate> GetAllAnalysisResultTemplateByUserId(Guid userId)
        {
            return Context.AnalysisResultTemplates.AsNoTracking()
                .Join(Context.ClinicSectionUsers.Where(a => a.UserId == userId),
                        clinicsectionUser => clinicsectionUser.ClinicSectionId,
                        analysis => analysis.ClinicSectionId,
                        (analysis, clinicsectionUser) => analysis);
                        
            ;
        }
    }
}
