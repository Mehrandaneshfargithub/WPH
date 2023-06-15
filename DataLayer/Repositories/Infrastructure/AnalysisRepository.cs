using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisRepository : Repository<Analysis>, IAnalysisRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Analysis> GetAllAnalysis(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                List<Analysis> Analysiss = Context.Analyses.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId)
                .Include(x => x.DiscountCurrency)
                .Include(x => x.ModifiedUser)
                .Include(x => x.CreateUser)
                .Include(x => x.AnalysisAnalysisItems).ThenInclude(a => a.AnalysisItem)
                .Select(x => new Analysis
                {
                    Guid = x.Guid,
                    Name = x.Name,
                    Code = x.Code,
                    Discount = x.Discount,
                    Abbreviation = x.Abbreviation,
                    DiscountCurrency = x.DiscountCurrency,
                    CreateDate = x.CreateDate,
                    CreateUser = x.CreateUser,
                    IsActive = x.IsActive,
                    ModifiedDate = x.ModifiedDate,
                    ModifiedUser = x.ModifiedUser,
                    Note = x.Note,
                    Priority = x.Priority,
                    AnalysisAnalysisItems = x.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                    {
                        AnalysisItem = new AnalysisItem
                        {
                            Amount = a.AnalysisItem.Amount
                        }
                    }).ToList()
                }).ToList();

                return Analysiss;
            }
            catch (Exception e) { throw e; }

        }

        public List<Analysis> GetAllAnalysisByUserId(Guid userId)
        {
            try
            {
                List<Analysis> Analysiss = Context.Analyses.AsNoTracking()
                .Include(x => x.DiscountCurrency)
                .Include(x => x.ModifiedUser)
                .Include(x => x.CreateUser)
                .Include(x => x.AnalysisAnalysisItems).ThenInclude(a => a.AnalysisItem)
                    .Join(Context.ClinicSectionUsers.Where(a => a.UserId == userId),
                        clinicsectionUser => clinicsectionUser.ClinicSectionId,
                        analysis => analysis.ClinicSectionId,
                        (analysis, clinicsectionUser) => new Analysis
                        {
                            Guid = analysis.Guid,
                            Name = analysis.Name,
                            Code = analysis.Code,
                            Discount = analysis.Discount,
                            Abbreviation = analysis.Abbreviation,
                            DiscountCurrency = analysis.DiscountCurrency,
                            CreateDate = analysis.CreateDate,
                            CreateUser = analysis.CreateUser,
                            IsActive = analysis.IsActive,
                            ModifiedDate = analysis.ModifiedDate,
                            ModifiedUser = analysis.ModifiedUser,
                            Note = analysis.Note,
                            Priority = analysis.Priority,
                            AnalysisAnalysisItems = analysis.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                            {
                                AnalysisItem = new AnalysisItem
                                {
                                    Amount = a.AnalysisItem.Amount
                                }
                            }).ToList()
                        }).ToList();

                return Analysiss;
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<Analysis> GetAllAnalysis()
        {
            return Context.Analyses.AsNoTracking();

        }

        public IEnumerable<Analysis> GetAllAnalysisWithAnalysisItems(Guid clinicSectionId, int DestCurrencyId)
        {
            try
            {

                return Context.Analyses
                    .AsNoTracking()
                    .Include(x => x.AnalysisAnalysisItems)
                    .ThenInclude(a => a.AnalysisItem)
                    .Where(x => x.ClinicSectionId == clinicSectionId && x.IsActive == true).OrderBy(x => x.Priority)
                    .Select(x => new Analysis
                    {
                        Guid = x.Guid,
                        Name = x.Name,
                        Code = x.Code,
                        Priority = x.Priority,
                        IsButton = x.IsButton,
                        IsActive = x.IsActive,
                        ClinicSectionId = x.ClinicSectionId,
                        DiscountCurrencyId = x.DiscountCurrencyId,

                        Discount = x.Discount,
                        //TotalAmount = x.AnalysisAnalysisItems.Sum(x => x.AnalysisItem.Amount),
                        AnalysisAnalysisItems = x.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                        {
                            AnalysisItem = (a.AnalysisItem == null) ? null : new AnalysisItem
                            {

                                Amount = a.AnalysisItem.Amount,

                            }
                        }).ToList()
                    });
            }
            catch (Exception e) { throw e; }

        }
        public IEnumerable<Analysis> GetAllAnalysisWithoutInGroupAnalysisAnalyses(Guid groupId, Guid clinicSectionId)
        {

            return Context.Analyses.AsNoTracking()
                .Where(c => !Context.GroupAnalysisAnalyses.Where(x => x.GroupAnalysisId == groupId).Select(b => b.AnalysisId).Contains(c.Guid) && c.ClinicSectionId == clinicSectionId);

        }

        public IEnumerable<Analysis> GetAllAnalysisWithoutInGroupAnalysis_AnalysisByUserId(Guid groupId)
        {
            var group = _context.GroupAnalyses.SingleOrDefault(p => p.Guid == groupId);

            return _context.Analyses.AsNoTracking()
                .Where(p => p.ClinicSectionId == group.ClinicSectionId &&
                !_context.GroupAnalysisAnalyses.Where(x => x.GroupAnalysisId == groupId).Select(x => x.AnalysisId).Contains(p.Guid));
        }

        public void RemoveAnalysis(Guid analysisId)
        {
            try
            {
                Analysis AnalysisItem = Context.Analyses.SingleOrDefault(a => a.Guid == analysisId);
                int? removedPriority = AnalysisItem.Priority;
                IEnumerable<Analysis> all = Context.Analyses.Where(x => x.Priority > removedPriority).Select(a => new Analysis
                {
                    Priority = a.Priority,
                    Guid = a.Guid
                }).OrderBy(a => a.Priority);

                foreach (var analysis in all)
                {
                    analysis.Priority -= 1;
                    Context.Entry(analysis).Property(x => x.Priority).IsModified = true;
                }
                Context.Entry(AnalysisItem).State = EntityState.Deleted;
                //Context.Entry(AnalysisItem).Property(x => x.Priority).IsModified = true;

                Context.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<Analysis> GetAllAnalysisByClinicSectionId(Guid clinicSectionId)
        {
            try
            {
                return Context.Analyses.AsNoTracking()
                    .Include(x => x.DiscountCurrency)
                .Include(x => x.ModifiedUser)
                .Include(x => x.CreateUser)
                .Include(x => x.AnalysisAnalysisItems).ThenInclude(a => a.AnalysisItem)
                .Where(a => a.ClinicSectionId == clinicSectionId)
                .OrderBy(a => a.Priority)
                .Select(analysis => new Analysis
                {
                    Guid = analysis.Guid,
                    Name = analysis.Name,
                    Code = analysis.Code,
                    Discount = analysis.Discount,
                    Abbreviation = analysis.Abbreviation,
                    DiscountCurrency = analysis.DiscountCurrency,
                    //CreateDate = analysis.CreateDate,
                    ClinicSectionId = analysis.ClinicSectionId,
                    DiscountCurrencyId = analysis.DiscountCurrencyId,
                    CreateUser = new User
                    {
                        Name = analysis.CreateUser.Name
                    },
                    IsActive = analysis.IsActive,
                    //ModifiedDate = analysis.ModifiedDate,
                    ModifiedUser = (analysis.ModifiedUser == null) ? null : new User
                    {
                        Name = analysis.ModifiedUser.Name
                    },
                    Note = analysis.Note,
                    Priority = analysis.Priority,
                    AnalysisAnalysisItems = analysis.AnalysisAnalysisItems.Select(a => new AnalysisAnalysisItem
                    {
                        AnalysisItem = new AnalysisItem
                        {
                            AmountCurrencyId = a.AnalysisItem.AmountCurrencyId,
                            Amount = a.AnalysisItem.Amount
                        }
                    }).ToList()
                });
            }
            catch (Exception e) { throw e; }
        }

        public async Task<List<Analysis>> GetAllAnalysisWithAnalysisItemAsync()
        {
            try
            {
                return await Context.Analyses.AsNoTracking()
                .Include(x => x.ModifiedUser)
                .Include(x => x.CreateUser)
                .Include(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Select(analysis => new Analysis
                {
                    Guid = analysis.Guid,
                    Name = analysis.Name,
                    Code = analysis.Code,
                    Discount = analysis.Discount,
                    Abbreviation = analysis.Abbreviation,
                    DiscountCurrency = analysis.DiscountCurrency,
                    ClinicSectionId = analysis.ClinicSectionId,
                    DiscountCurrencyId = analysis.DiscountCurrencyId,
                    CreateDate = analysis.CreateDate,
                    CreateUserId = analysis.CreateUserId,
                    IsButton = analysis.IsButton,
                    ModifiedDate = analysis.ModifiedDate,
                    ModifiedUserId = analysis.ModifiedUserId,
                    CreateUser = new User
                    {
                        Name = analysis.CreateUser.Name
                    },
                    IsActive = analysis.IsActive,
                    ModifiedUser = (analysis.ModifiedUser == null) ? null : new User
                    {
                        Name = analysis.ModifiedUser.Name
                    },
                    Note = analysis.Note,
                    Priority = analysis.Priority,
                    AnalysisAnalysisItems = analysis.AnalysisAnalysisItems.OrderBy(x => x.Priority).ToList()
                }).ToListAsync();
            }
            catch (Exception e) { throw e; }
        }

        public List<Analysis> GetAllAnalysisWithAnalysisItem()
        {
            try
            {
                return Context.Analyses.AsNoTracking()
                .Include(x => x.ModifiedUser)
                .Include(x => x.CreateUser)
                .Include(x => x.AnalysisAnalysisItems)
                .Select(analysis => new Analysis
                {
                    Guid = analysis.Guid,
                    Name = analysis.Name,
                    Code = analysis.Code,
                    Discount = analysis.Discount,
                    Abbreviation = analysis.Abbreviation,
                    DiscountCurrency = analysis.DiscountCurrency,
                    ClinicSectionId = analysis.ClinicSectionId,
                    DiscountCurrencyId = analysis.DiscountCurrencyId,
                    CreateDate = analysis.CreateDate,
                    CreateUserId = analysis.CreateUserId,
                    IsButton = analysis.IsButton,
                    ModifiedDate = analysis.ModifiedDate,
                    ModifiedUserId = analysis.ModifiedUserId,
                    CreateUser = new User
                    {
                        Name = analysis.CreateUser.Name
                    },
                    IsActive = analysis.IsActive,
                    ModifiedUser = (analysis.ModifiedUser == null) ? null : new User
                    {
                        Name = analysis.ModifiedUser.Name
                    },
                    Note = analysis.Note,
                    Priority = analysis.Priority,
                    AnalysisAnalysisItems = analysis.AnalysisAnalysisItems.OrderBy(x => x.Priority).ToList()
                }).ToList();
            }
            catch (Exception e) { throw e; }
        }
    }
}
