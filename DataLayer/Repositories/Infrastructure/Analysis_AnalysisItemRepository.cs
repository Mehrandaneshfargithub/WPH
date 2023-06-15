using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisAnalysisItemRepository : Repository<AnalysisAnalysisItem>, IAnalysisAnalysisItemRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisAnalysisItemRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<AnalysisAnalysisItem> GetAllAnalysisAnalysisItem(Guid analysisId)
        {
            return Context.AnalysisAnalysisItems
                .Include(x => x.AnalysisItem).ThenInclude(a => a.ValueType)
                .Include(x => x.AnalysisItem).ThenInclude(a => a.Unit)
                .Include(x => x.AnalysisItem).ThenInclude(a => a.AmountCurrency)
                .AsNoTracking()
                .Where(x => x.AnalysisId == analysisId).OrderBy(x => x.Priority)
                .Select(a => new AnalysisAnalysisItem
                {
                    Guid = a.Guid,
                    Priority = a.Priority,
                    AnalysisItem = new AnalysisItem
                    {
                        ClinicSectionId = a.AnalysisItem.ClinicSectionId,
                        AmountCurrencyId = a.AnalysisItem.AmountCurrencyId,
                        AmountCurrency = new BaseInfoGeneral
                        {
                            Name = a.AnalysisItem.AmountCurrency.Name
                        },
                        Unit = new BaseInfo
                        {
                            Name = a.AnalysisItem.Unit.Name
                        },
                        ValueType = new BaseInfoGeneral
                        {
                            Name = a.AnalysisItem.ValueType.Name
                        },
                        Name = a.AnalysisItem.Name,
                        Code = a.AnalysisItem.Code,
                        Abbreviation = a.AnalysisItem.Abbreviation,
                        Note = a.AnalysisItem.Note,
                        NormalValues = a.AnalysisItem.NormalValues,
                        Amount = a.AnalysisItem.Amount
                    }
                })
                ;
        }

        public void RemoveAnalysisAnalysisItem(Guid analysisItemId)
        {
            AnalysisAnalysisItem ana = new AnalysisAnalysisItem() { Guid = analysisItemId };
            Context.AnalysisAnalysisItems.Attach(ana);
            Context.Entry(ana).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void UpdatePriority(AnalysisAnalysisItem currentAnalysisItem)
        {
            //Context.Entry(currentAnalysisItem).State = EntityState.Modified;
            Context.Entry(currentAnalysisItem).Property(x => x.Priority).IsModified = true;
        }

        public void RemoveAnalysisItemFromAnalysis(Guid id)
        {
            try
            {
                AnalysisAnalysisItem AnalysisItem = Context.AnalysisAnalysisItems.SingleOrDefault(a => a.Guid == id);
                int? removedPriority = AnalysisItem.Priority;
                IEnumerable<AnalysisAnalysisItem> all = Context.AnalysisAnalysisItems.Where(x => x.Priority > removedPriority && x.AnalysisId == AnalysisItem.AnalysisId).Select(a => new AnalysisAnalysisItem
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
    }
}
