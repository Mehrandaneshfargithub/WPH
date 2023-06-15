using DataLayer.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisItemRepository : Repository<AnalysisItem>, IAnalysisItemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisItemRepository(WASContext context)
            : base(context)
        {
        }


        public List<AnalysisItem> GetAllAnalysisItemWithoutInGroupAnalysisItemByUserId(Guid groupId)
        {
            var group = _context.GroupAnalyses.SingleOrDefault(p => p.Guid == groupId);

            return _context.AnalysisItems.AsNoTracking()
                .Where(p=>p.ClinicSectionId==group.ClinicSectionId &&
                !_context.GroupAnalysisItems.Where(x => x.GroupAnalysisId == groupId).Select(x => x.AnalysisItemId).Contains(p.Guid))
                .OrderBy(x => x.Priority).ToList();
        }

        public void UpdatePriority(AnalysisItem currentAnalysisItem)
        {
            Context.Entry(currentAnalysisItem).State = EntityState.Modified;
            Context.Entry(currentAnalysisItem).Property(x => x.Priority).IsModified = true;
        }

        public IEnumerable<AnalysisItem> GetAllAnalysisItem(Guid clinicSectionId)
        {
            return Context.AnalysisItems.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId)
                .Include(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.AnalysisItemValuesRanges)
                .Include(x => x.ValueType)
                .Include(x => x.AmountCurrency)
                .Include(x => x.Unit)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser)
                .OrderBy(x => x.Priority);
        }

        public async Task<List<AnalysisItem>> GetAllAnalysisItemAsync()
        {
            return await Context.AnalysisItems.AsNoTracking()
                .Include(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.AnalysisItemValuesRanges)
                .Include(x => x.ValueType)
                //.Include(x => x.AmountCurrency)
                .Include(x => x.Unit)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser).Select(b => new AnalysisItem
                {
                    Priority = b.Priority,
                    Guid = b.Guid,
                    Name = b.Name,
                    Code = b.Code,
                    Abbreviation = b.Abbreviation,
                    Note = b.Note,
                    NormalValues = b.NormalValues,
                    Amount = b.Amount,
                    ClinicSectionId = b.ClinicSectionId,
                    AnalysisItemMinMaxValues = b.AnalysisItemMinMaxValues,
                    AnalysisItemValuesRanges = b.AnalysisItemValuesRanges.OrderByDescending(x => x.Default).ToList(),
                    CreatedDate = b.CreatedDate,
                    CreatedUserId = b.CreatedUserId,
                    IsButton = b.IsButton,
                    ModifiedDate = b.ModifiedDate,
                    ModifiedUserId = b.ModifiedUserId,
                    UnitId = b.UnitId,
                    ValueTypeId = b.ValueTypeId,
                    ShowChart = b.ShowChart,
                    CreatedUser = new User
                    {
                        Name = b.CreatedUser.Name
                    },
                    ModifiedUser = (b.ModifiedUser == null) ? null : new User
                    {
                        Name = b.ModifiedUser.Name
                    },
                    Unit = (b.Unit == null) ? null : new BaseInfo
                    {
                        Name = b.Unit.Name
                    },
                    ValueType = (b.ValueType == null) ? null : new BaseInfoGeneral
                    {
                        Name = b.ValueType.Name
                    }
                }).OrderBy(x => x.Priority).ToListAsync();
        }

        public IEnumerable<AnalysisItem> GetAllAnalysisItemByClinicSectionId(Guid clinicSectionId)
        {
            return Context.AnalysisItems.AsNoTracking()
                .Include(x => x.ValueType)
                //.Include(x => x.AmountCurrency)
                .Include(x => x.Unit)
                .Include(x => x.CreatedUser)
                .Include(x => x.ModifiedUser)
                .Where(a => a.ClinicSectionId == clinicSectionId).OrderBy(x => x.Priority)
                .Select(b => new AnalysisItem
                {
                    Priority = b.Priority,
                    Guid = b.Guid,
                    Name = b.Name,
                    Code = b.Code,
                    Abbreviation = b.Abbreviation,
                    Note = b.Note,
                    NormalValues = b.NormalValues,
                    Amount = b.Amount,
                    ClinicSectionId = b.ClinicSectionId,
                    ShowChart = b.ShowChart,
                    CreatedUser = new User
                    {
                        Name = b.CreatedUser.Name
                    },
                    ModifiedUser = (b.ModifiedUser == null) ? null : new User
                    {
                        Name = b.ModifiedUser.Name
                    },
                    Unit = (b.Unit == null) ? null : new BaseInfo
                    {
                        Name = b.Unit.Name
                    },
                    ValueType = (b.ValueType == null) ? null : new BaseInfoGeneral
                    {
                        Name = b.ValueType.Name
                    }
                });

        }

        public AnalysisItem GetAnalysisItemBasedOnId(Guid analysisItemId)
        {
            return Context.AnalysisItems
                .Include(x => x.ValueType)
                .Include(x => x.Unit)
                .Include(x => x.AnalysisItemValuesRanges)
                .Include(x => x.AnalysisItemMinMaxValues)
                .SingleOrDefault(x => x.Guid == analysisItemId);
        }

        public List<AnalysisItem> GetAllAnalysisItemWithoutInAnalysisByUserId(Guid analysisId, Guid userId)
        {
            return Context.AnalysisItems.AsNoTracking().Include(a => a.AmountCurrency).Include(a => a.Unit).Include(a => a.ValueType)
                .Join(Context.ClinicSectionUsers.Where(a => a.UserId == userId),
                        clinicsectionUser => clinicsectionUser.ClinicSectionId,
                        analysis => analysis.ClinicSectionId,
                        (analysis, clinicsectionUser) => analysis)
                        .Where(c => !Context.AnalysisAnalysisItems.Where(x => x.AnalysisId == analysisId)
                                                     .Select(b => b.AnalysisItemId)
                                                     .Contains(c.Guid))
                        .Select(a => new AnalysisItem
                        {
                            Guid = a.Guid,
                            Name = a.Name,
                            Code = a.Code,
                            Note = a.Note,
                            NormalValues = a.NormalValues,
                            Amount = a.Amount,
                            Abbreviation = a.Abbreviation,
                            Priority = a.Priority,
                            AmountCurrency = new BaseInfoGeneral
                            {
                                Name = a.AmountCurrency.Name
                            },
                            Unit = new BaseInfo
                            {
                                Name = a.Unit.Name
                            },
                            ValueType = new BaseInfoGeneral
                            {
                                Name = a.ValueType.Name
                            }

                        }).OrderBy(a => a.Priority)
                        .ToList();
        }

        public List<AnalysisItem> GetAllAnalysisItemsWithNameAndGuidOnly(Guid clinicSectionId, int destCurrencyId)
        {
            return Context.AnalysisItems.AsNoTracking()
                .Where(x => x.ClinicSectionId == clinicSectionId)
                .OrderBy(x => x.Priority)
                .Select(x => new AnalysisItem
                {
                    Guid = x.Guid,
                    Name = x.Name,
                    Code = x.Code,
                    Priority = x.Priority,
                    IsButton = x.IsButton,
                    ClinicSectionId = x.ClinicSectionId,
                    Amount = x.Amount
                }).ToList();
        }



        public void RemoveAnalysisItem(Guid AnalysisItemId)
        {
            try
            {
                AnalysisItem AnalysisItem = Context.AnalysisItems.SingleOrDefault(a => a.Guid == AnalysisItemId);
                int? removedPriority = AnalysisItem.Priority;
                IEnumerable<AnalysisItem> all = Context.AnalysisItems.Where(x => x.Priority > removedPriority).Select(a => new AnalysisItem
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

        public AnalysisItem GetWithValueType(Guid analysisItemId)
        {
            return _context.AnalysisItems.AsNoTracking()
                .Include(p => p.ValueType)
                .SingleOrDefault(p => p.Guid == analysisItemId);
        }


    }
}
