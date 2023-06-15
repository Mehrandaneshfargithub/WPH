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
    public class PatientReceptionAnalysisRepository : Repository<PatientReceptionAnalysis>, IPatientReceptionAnalysisRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }

        public PatientReceptionAnalysisRepository(WASContext context)
                   : base(context)
        {
        }

        public List<PatientReceptionAnalysis> GetPatientReceptionAnalysisByReceptionId(Guid receptionId)
        {

            //Reception rece = _context.Receptions
            //    .Select(a => new Reception
            //    {

            //        Guid = a.Guid,
            //        ClinicSectionId = a.ClinicSectionId,
            //        DiscountCurrencyId = a.DiscountCurrencyId

            //    }).SingleOrDefault(x => x.Guid == receptionId);

            return _context.PatientReceptionAnalyses
                .Include(a => a.Analysis)
                .Include(a => a.AnalysisItem)
                .Include(a => a.GroupAnalysis)
                .Include(a => a.ModifiedUser)
                .AsNoTracking().Where(x => x.ReceptionId == receptionId)
                .Select(a => new PatientReceptionAnalysis
                {
                    Guid = a.Guid,
                    Amount = a.Amount,
                    AmountCurrency = a.AmountCurrency,
                    AmountCurrencyId = a.AmountCurrencyId,
                    Discount = a.Discount,
                    Analysis = new Analysis
                    {
                        Name = a.Analysis == null ? null : a.Analysis.Name,
                        Code = a.Analysis == null ? null : a.Analysis.Code,
                    },
                    AnalysisId = a.AnalysisId,
                    AnalysisItem = new AnalysisItem
                    {
                        Name = a.AnalysisItem == null ? null : a.AnalysisItem.Name,
                        Code = a.AnalysisItem == null ? null : a.AnalysisItem.Code,
                    },
                    AnalysisItemId = a.AnalysisItemId,
                    GroupAnalysis = new GroupAnalysis
                    {
                        Name = a.GroupAnalysis == null ? null : a.GroupAnalysis.Name,
                        Code = a.GroupAnalysis == null ? null : a.GroupAnalysis.Code,
                    },
                    GroupAnalysisId = a.GroupAnalysisId,
                }).ToList();

        }

        public List<PatientReceptionAnalysis> GetAllPatientReceptionAnalysis(Guid receptionId)
        {
            return Context.PatientReceptionAnalyses.AsNoTracking()
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.ValueType)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.ValueType)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.ValueType)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemMinMaxValues)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.AnalysisItemValuesRanges)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.ValueType)
                .Where(x => x.ReceptionId == receptionId)
                .Select(a => new PatientReceptionAnalysis
                {
                    ReceptionId = a.ReceptionId,
                    AmountCurrencyId = a.AmountCurrencyId,
                    Analysis = a.Analysis == null ? null : new Analysis
                    {
                        Guid = a.Analysis.Guid,
                        Name = a.Analysis.Name,
                        AnalysisAnalysisItems = a.Analysis.AnalysisAnalysisItems.Select(b => new AnalysisAnalysisItem
                        {
                            Priority = b.Priority,
                            AnalysisId = b.AnalysisId,
                            AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                            {
                                Guid = b.AnalysisItem.Guid,
                                ValueType = b.AnalysisItem.ValueType,
                                Name = b.AnalysisItem.Name,
                                AnalysisItemMinMaxValues = b.AnalysisItem.AnalysisItemMinMaxValues,
                                AnalysisItemValuesRanges = b.AnalysisItem.AnalysisItemValuesRanges.OrderByDescending(a => a.Default).ToList(),
                                NormalValues = b.AnalysisItem.NormalValues,
                                ShowChart = b.AnalysisItem.ShowChart,
                                Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                {
                                    Name = b.AnalysisItem.Unit.Name
                                }
                            }
                        }).OrderBy(a => a.Priority).ToList()
                    },
                    AnalysisItem = a.AnalysisItem == null ? null : new AnalysisItem
                    {
                        Guid = a.AnalysisItem.Guid,
                        ValueType = a.AnalysisItem.ValueType,
                        Name = a.AnalysisItem.Name,
                        AnalysisItemMinMaxValues = a.AnalysisItem.AnalysisItemMinMaxValues,
                        AnalysisItemValuesRanges = a.AnalysisItem.AnalysisItemValuesRanges.OrderByDescending(a => a.Default).ToList(),
                        NormalValues = a.AnalysisItem.NormalValues,
                        ShowChart = a.AnalysisItem.ShowChart,
                        Unit = a.AnalysisItem.Unit == null ? null : new BaseInfo
                        {
                            Name = a.AnalysisItem.Unit.Name
                        }

                    },
                    GroupAnalysis = a.GroupAnalysis == null ? null : new GroupAnalysis
                    {
                        Guid = a.GroupAnalysis.Guid,
                        Name = a.GroupAnalysis.Name,
                        GroupAnalysisItems = a.GroupAnalysis.GroupAnalysisItems.Select(b => new GroupAnalysisItem
                        {
                            Guid = b.Guid,
                            AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                            {
                                Guid = b.AnalysisItem.Guid,
                                ValueType = b.AnalysisItem.ValueType,
                                Name = b.AnalysisItem.Name,
                                AnalysisItemMinMaxValues = b.AnalysisItem.AnalysisItemMinMaxValues,
                                AnalysisItemValuesRanges = b.AnalysisItem.AnalysisItemValuesRanges.OrderByDescending(a => a.Default).ToList(),
                                NormalValues = b.AnalysisItem.NormalValues,
                                ShowChart = b.AnalysisItem.ShowChart,
                                Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                {
                                    Name = b.AnalysisItem.Unit.Name
                                }
                            },
                            GroupAnalysisId = b.GroupAnalysisId,
                            AnalysisItemId = b.AnalysisItemId
                        }).ToList(),
                        GroupAnalysisAnalyses = a.GroupAnalysis.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                        {
                            AnalysisId = b.AnalysisId,
                            GroupAnalysisId = b.GroupAnalysisId,
                            Guid = b.Guid,
                            Analysis = b.Analysis == null ? null : new Analysis
                            {
                                Name = b.Analysis.Name,
                                AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(c => new AnalysisAnalysisItem
                                {
                                    AnalysisId = c.AnalysisId,
                                    AnalysisItem = c.AnalysisItem == null ? null : new AnalysisItem
                                    {
                                        Guid = c.AnalysisItem.Guid,
                                        ValueType = c.AnalysisItem.ValueType,
                                        Name = c.AnalysisItem.Name,
                                        AnalysisItemMinMaxValues = c.AnalysisItem.AnalysisItemMinMaxValues,
                                        AnalysisItemValuesRanges = c.AnalysisItem.AnalysisItemValuesRanges.OrderByDescending(a => a.Default).ToList(),
                                        NormalValues = c.AnalysisItem.NormalValues,
                                        ShowChart = c.AnalysisItem.ShowChart,
                                        Unit = c.AnalysisItem.Unit == null ? null : new BaseInfo
                                        {
                                            Name = c.AnalysisItem.Unit.Name
                                        }
                                    }
                                }).ToList()
                            },
                        }).ToList()
                    }
                })
                .ToList();


        }


        public List<PatientReceptionAnalysis> GetPatientReceptionAnalysisByReceptionIdJustIds(Guid receptionId)
        {

            return _context.PatientReceptionAnalyses
                .AsNoTracking().Where(x => x.ReceptionId == receptionId)
                .Select(a => new PatientReceptionAnalysis
                {
                    ReceptionId = a.ReceptionId,
                    AmountCurrencyId = a.AmountCurrencyId,
                    AnalysisId = a.AnalysisId,
                    AnalysisItemId = a.AnalysisItemId,
                    GroupAnalysisId = a.GroupAnalysisId,
                }).ToList();

        }

        public IEnumerable<PatientReceptionAnalysis> GetAllPatientReceptionAnalysisByClinicSectionIds(IEnumerable<Guid> allReceptionIds, DateTime dateFrom, DateTime dateTo)
        {
            return _context.PatientReceptionAnalyses.Include(x => x.Reception)
                .AsNoTracking().Where(x => allReceptionIds.Contains(x.ReceptionId) && x.Reception.ReceptionDate <= dateTo && x.Reception.ReceptionDate >= dateFrom)
                .Select(a => new PatientReceptionAnalysis
                {
                    AnalysisId = a.AnalysisId,
                    AnalysisItemId = a.AnalysisItemId,
                    GroupAnalysisId = a.GroupAnalysisId,
                });

        }

        public IEnumerable<PieChartModel> GetMostUsedAnalysis(Guid userId)
        {
            return Context.PatientReceptionAnalyses
                .Include(a => a.AnalysisItem)
                .Include(a => a.Reception)
                .Where(a => !string.IsNullOrWhiteSpace(a.AnalysisItem.Name) && Context.ClinicSectionUsers.Where(a => a.UserId == userId).Select(a => a.ClinicSectionId).Contains(a.Reception.ClinicSectionId.Value))
                .GroupBy(a => a.AnalysisItem.Name).Select(a => new PieChartModel
                {
                    Label = a.Key,
                    Value = a.Count()
                });
        }
    }
}
