using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class GroupAnalysisAnalysesRepository : Repository<GroupAnalysisAnalysis>, IGroupAnalysisAnalysesRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public GroupAnalysisAnalysesRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<GroupAnalysisAnalysis> GetAllGroupAnalysisAnalyses(Guid groupAnalysisId)
        {
            return Context.GroupAnalysisAnalyses.AsNoTracking()
                .Include(x => x.GroupAnalysis)
                .Include(x => x.Analysis)
                .Where(x => x.GroupAnalysisId == groupAnalysisId).OrderBy(x => x.Priority).Select(x => new GroupAnalysisAnalysis
                {
                    Guid = x.Guid,
                    Analysis = new Analysis
                    {
                        Guid = x.Analysis.Guid,
                        Name = x.Analysis.Name,
                        Code = x.Analysis.Code,
                        Discount = x.Analysis.Discount,
                        Abbreviation = x.Analysis.Abbreviation,
                        DiscountCurrency = x.Analysis.DiscountCurrency,
                        CreateDate = x.Analysis.CreateDate,
                        CreateUser = x.Analysis.CreateUser,
                        IsActive = x.Analysis.IsActive,
                        ModifiedDate = x.Analysis.ModifiedDate,
                        ModifiedUser = x.Analysis.ModifiedUser,
                        Note = x.Analysis.Note,
                        AnalysisAnalysisItems = x.Analysis.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                        { AnalysisItem = new AnalysisItem { Amount = a.AnalysisItem.Amount } }).ToList()
                    }


                });

        }

        public IEnumerable<GroupAnalysisAnalysis> GetAllGroupAnalysisAnalyses()
        {
            return Context.GroupAnalysisAnalyses
                .AsNoTracking()
                .Include(x => x.GroupAnalysis)
                .Include(x => x.Analysis).OrderBy(x => x.Priority);

        }

        public void UpdatePriority(GroupAnalysisAnalysis currentGroupAnalysisAnalyses)
        {
            Context.Entry(currentGroupAnalysisAnalyses).State = EntityState.Modified;
            Context.Entry(currentGroupAnalysisAnalyses).Property(x => x.Priority).IsModified = true;
        }
    }
}
