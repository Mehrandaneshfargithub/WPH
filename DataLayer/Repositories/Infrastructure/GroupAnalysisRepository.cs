using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class GroupAnalysisRepository : Repository<GroupAnalysis>, IGroupAnalysisRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public GroupAnalysisRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<GroupAnalysis> GetAllGroupAnalysis(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {
            return Context.GroupAnalyses
                .AsNoTracking()
                .Include(x => x.DiscountCurrency)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser)
                .OrderBy(x => x.Priority)
                .Include(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Where(p => p.ClinicSectionId == clinicSectionId)
                .Select(x => new GroupAnalysis
                {
                    Name = x.Name,
                    Guid = x.Guid,
                    Discount = x.Discount,
                    Code = x.Code,
                    Abbreviation = x.Abbreviation,
                    DiscountCurrency = x.DiscountCurrency,
                    CreatedDate = x.CreatedDate,
                    CreatedUser = x.CreatedUser,
                    IsActive = x.IsActive,
                    ModifiedDate = x.ModifiedDate,
                    ModifiedUser = x.ModifiedUser,
                    Note = x.Note,
                    Priority = x.Priority,
                    GroupAnalysisItems = x.GroupAnalysisItems.Select(a => new GroupAnalysisItem
                    {

                        AnalysisItem = new AnalysisItem { Amount = a.AnalysisItem.Amount }
                    }).ToList(),
                    GroupAnalysisAnalyses = x.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                    {

                        Analysis = new Analysis
                        {
                            Discount = b.Analysis.Discount,
                            AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                            {
                                AnalysisItem = new AnalysisItem { Amount = a.AnalysisItem.Amount }
                            }).ToList()
                        }

                    }).ToList()
                }).ToList();

        }

        public IEnumerable<GroupAnalysis> GetAllGroupAnalysis(Guid clinicSectionId)
        {

            return Context.GroupAnalyses.AsNoTracking().Where(x => x.ClinicSectionId == clinicSectionId).OrderBy(x => x.Priority);
        }

        public void UpdatePriority(GroupAnalysis currentGroupAnalysis)
        {
            Context.Entry(currentGroupAnalysis).State = EntityState.Modified;
            Context.Entry(currentGroupAnalysis).Property(x => x.Priority).IsModified = true;
        }

        public IEnumerable<GroupAnalysis> GetAllGroupAnalysis()
        {
            return Context.GroupAnalyses;

        }

        public List<GroupAnalysis> GetAllGroupAnalysisWithNameAndGuidOnly(Guid clinicSectionId, int DestCurrencyId)
        {
            return Context.GroupAnalyses.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId && x.IsActive == true).OrderBy(x => x.Priority)
                .Include(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Select(x => new GroupAnalysis
                {
                    Name = x.Name,
                    Guid = x.Guid,
                    IsButton = x.IsButton,
                    Priority = x.Priority,
                    ClinicSectionId = x.ClinicSectionId,
                    IsActive = x.IsActive,

                    Discount = x.Discount,
                    Code = x.Code,
                    GroupAnalysisItems = x.GroupAnalysisItems.Select(a => new GroupAnalysisItem
                    {
                        AnalysisItemId = a.AnalysisItemId ?? Guid.Empty,
                        AnalysisItem = new AnalysisItem
                        {

                            Amount = a.AnalysisItem.Amount
                        }
                    }).ToList(),
                    GroupAnalysisAnalyses = x.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                    {
                        AnalysisId = b.AnalysisId ?? Guid.Empty,

                        Analysis = new Analysis
                        {
                            Discount = b.Analysis.Discount,

                            AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                            {
                                AnalysisItem = new AnalysisItem
                                {

                                    Amount = a.AnalysisItem.Amount
                                }
                            }).ToList()
                        }

                    }).ToList()
                }).ToList();
        }

        public async Task<List<GroupAnalysis>> GetAllGroupAnalysisWithAnalysisAndAnalysisItemAsync()
        {
            return await Context.GroupAnalyses
                .AsNoTracking()
                .Include(x => x.DiscountCurrency)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser)
                .Include(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .OrderBy(x => x.Priority)
                .Select(x => new GroupAnalysis
                {
                    Name = x.Name,
                    Guid = x.Guid,
                    Discount = x.Discount,
                    Code = x.Code,
                    Abbreviation = x.Abbreviation,
                    DiscountCurrency = x.DiscountCurrency,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                    ModifiedDate = x.ModifiedDate,
                    CreatedUser = new User
                    {
                        Name = x.CreatedUser.Name
                    },
                    ModifiedUser = (x.ModifiedUser == null) ? null : new User
                    {
                        Name = x.ModifiedUser.Name
                    },
                    Note = x.Note,
                    Priority = x.Priority,
                    ClinicSectionId = x.ClinicSectionId,
                    CreatedUserId = x.CreatedUserId,
                    DiscountCurrencyId = x.DiscountCurrencyId,
                    ModifiedUserId = x.ModifiedUserId,
                    IsButton = x.IsButton,
                    GroupAnalysisItems = x.GroupAnalysisItems,
                    GroupAnalysisAnalyses = x.GroupAnalysisAnalyses
                }).ToListAsync();
        }

        public List<GroupAnalysis> GetAllGroupAnalysisWithAnalysisAndAnalysisItem()
        {
            return Context.GroupAnalyses
                .AsNoTracking()
                .Include(x => x.DiscountCurrency)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser)
                .Include(x => x.GroupAnalysisAnalyses)
                .Include(x => x.GroupAnalysisItems)
                .OrderBy(x => x.Priority)
                .Select(x => new GroupAnalysis
                {
                    Name = x.Name,
                    Guid = x.Guid,
                    Discount = x.Discount,
                    Code = x.Code,
                    Abbreviation = x.Abbreviation,
                    DiscountCurrency = x.DiscountCurrency,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                    ModifiedDate = x.ModifiedDate,
                    CreatedUser = new User
                    {
                        Name = x.CreatedUser.Name
                    },
                    ModifiedUser = (x.ModifiedUser == null) ? null : new User
                    {
                        Name = x.ModifiedUser.Name
                    },
                    Note = x.Note,
                    Priority = x.Priority,
                    ClinicSectionId = x.ClinicSectionId,
                    CreatedUserId = x.CreatedUserId,
                    DiscountCurrencyId = x.DiscountCurrencyId,
                    ModifiedUserId = x.ModifiedUserId,
                    IsButton = x.IsButton,
                    GroupAnalysisItems = x.GroupAnalysisItems,
                    GroupAnalysisAnalyses = x.GroupAnalysisAnalyses
                }).ToList();
        }
    }
}
