using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLayer.Repositories.Infrastructure
{
    public class AnalysisResultRepository : Repository<AnalysisResult>, IAnalysisResultRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public AnalysisResultRepository(WASContext context)
            : base(context)
        {
        }

        public void AddNewRange(IEnumerable<AnalysisResult> analysisResult)
        {
            Guid AnalysisResultMasterId = analysisResult.FirstOrDefault().AnalysisResultMasterId ?? Guid.Empty;
            _context.AnalysisResults.RemoveRange(_context.AnalysisResults.Where(x => x.AnalysisResultMasterId == AnalysisResultMasterId));
            _context.AnalysisResults.AddRange(analysisResult);
            _context.SaveChanges();
        }

        public IEnumerable<AnalysisResult> GetAnalysisResultForReport(Guid AnalysisResultMasterId)
        {
            return _context.AnalysisResults.AsNoTracking()
                .Where(x => x.AnalysisResultMasterId == AnalysisResultMasterId)
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem)
                .Include(x => x.AnalysisItem)
                .Select(a => new AnalysisResult
                {
                    Value = a.Value,
                    Analysis = a.Analysis == null ? null : new Analysis
                    {
                        AnalysisAnalysisItems = a.Analysis.AnalysisAnalysisItems.Select(b => new AnalysisAnalysisItem
                        {
                            AnalysisItem = new AnalysisItem
                            {
                                Name = b.AnalysisItem.Name,
                                NormalValues = b.AnalysisItem.NormalValues
                            }
                        }).ToList(),
                        Name = a.Analysis.Name
                    },
                    AnalysisItem = a.AnalysisItem == null ? null : new AnalysisItem
                    {
                        Name = a.AnalysisItem.Name,
                        NormalValues = a.AnalysisItem.NormalValues
                    },
                    GroupAnalysis = a.GroupAnalysis == null ? null : new GroupAnalysis
                    {
                        GroupAnalysisItems = a.GroupAnalysis.GroupAnalysisItems.Select(b => new GroupAnalysisItem
                        {
                            AnalysisItem = new AnalysisItem
                            {
                                Name = b.AnalysisItem.Name,
                                NormalValues = b.AnalysisItem.NormalValues
                            }
                        }).ToList(),
                        GroupAnalysisAnalyses = a.GroupAnalysis.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                        {
                            Analysis = new Analysis
                            {
                                AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(c => new AnalysisAnalysisItem
                                {
                                    AnalysisItem = new AnalysisItem
                                    {
                                        Name = c.AnalysisItem.Name,
                                        NormalValues = c.AnalysisItem.NormalValues
                                    }
                                }).ToList()
                            }
                        }).ToList(),
                        Name = a.GroupAnalysis.Name
                    }
                });
        }

        public IEnumerable<AnalysisResult> GetPastAnalysisResults(Guid patientId, List<Guid> analysisItemId)
        {
            try
            {
                return _context.AnalysisResults.AsNoTracking()
                .Include(x => x.AnalysisItem)
                .Where(x => x.AnalysisResultMaster.Reception.PatientId == patientId && analysisItemId.Contains(x.AnalysisItemId ?? Guid.Empty))
                .OrderByDescending(x => x.CreatedDate).ThenBy(x => x.AnalysisItem.Name).Take(3)
                .Select(x => new AnalysisResult
                {
                    Value = x.Value,
                    CreatedDate = x.CreatedDate,
                    AnalysisItem = x.AnalysisItem == null ? null : new AnalysisItem
                    {
                        Name = x.AnalysisItem.Name,
                    }
                }).ToList();
            }
            catch (Exception e) { throw e; }

        }

        public List<AnalysisResult> GetAnalysisResultForAnalysisResultReport(Guid analysisResultMasterId)
        {

            return Context.AnalysisResults.AsNoTracking().Where(x => x.AnalysisResultMasterId == analysisResultMasterId)
                .Include(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisAnalyses).ThenInclude(x => x.Analysis).ThenInclude(x => x.AnalysisAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Include(x => x.GroupAnalysis).ThenInclude(x => x.GroupAnalysisItems).ThenInclude(x => x.AnalysisItem).ThenInclude(x => x.Unit)
                .Select(a => new AnalysisResult
                {
                    Value = a.Value,

                    Analysis = a.Analysis == null ? null : new Analysis
                    {
                        AnalysisAnalysisItems = a.Analysis.AnalysisAnalysisItems.Select(b => new AnalysisAnalysisItem
                        {
                            AnalysisItemId = b.AnalysisItemId,
                            Priority = b.Priority,
                            AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                            {
                                Name = b.AnalysisItem.Name,
                                NormalValues = b.AnalysisItem.NormalValues,
                                Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                {
                                    Name = b.AnalysisItem.Unit.Name
                                }
                            }
                        }).OrderBy(a => a.Priority).ToList(),
                        Name = a.Analysis.Name
                    },
                    AnalysisId = a.AnalysisId,
                    AnalysisItemId = a.AnalysisItemId,
                    AnalysisItem = a.AnalysisItem == null ? null : new AnalysisItem
                    {
                        Name = a.AnalysisItem.Name,
                        NormalValues = a.AnalysisItem.NormalValues,
                        Unit = a.AnalysisItem.Unit == null ? null : new BaseInfo
                        {
                            Name = a.AnalysisItem.Unit.Name
                        }
                    },
                    GroupAnalysis = a.GroupAnalysis == null ? null : new GroupAnalysis
                    {
                        GroupAnalysisItems = a.GroupAnalysis.GroupAnalysisItems.Select(b => new GroupAnalysisItem
                        {
                            AnalysisItem = b.AnalysisItem == null ? null : new AnalysisItem
                            {
                                Name = b.AnalysisItem.Name,
                                NormalValues = b.AnalysisItem.NormalValues,
                                Unit = b.AnalysisItem.Unit == null ? null : new BaseInfo
                                {
                                    Name = b.AnalysisItem.Unit.Name
                                }
                            }
                        }).ToList(),
                        GroupAnalysisAnalyses = a.GroupAnalysis.GroupAnalysisAnalyses.Select(b => new GroupAnalysisAnalysis
                        {
                            Analysis = b.Analysis == null ? null : new Analysis
                            {
                                AnalysisAnalysisItems = b.Analysis.AnalysisAnalysisItems.Select(c => new AnalysisAnalysisItem
                                {
                                    AnalysisItem = c.AnalysisItem == null ? null : new AnalysisItem
                                    {
                                        Name = c.AnalysisItem.Name,
                                        NormalValues = c.AnalysisItem.NormalValues,
                                        Unit = c.AnalysisItem.Unit == null ? null : new BaseInfo
                                        {
                                            Name = c.AnalysisItem.Unit.Name
                                        }
                                    }
                                }).ToList()
                            }
                        }).ToList(),
                        Name = a.GroupAnalysis.Name
                    }
                }).OrderBy(a => a.AnalysisId).ToList();
        }

        public IEnumerable<AnalysisResult> GetAnalysisResultHistoryForChart(Guid patientId, DateTime invoiceDate, List<Guid?> analysisItems)
        {
            return _context.AnalysisResults.AsNoTracking()
                .Include(p => p.AnalysisResultMaster).ThenInclude(p => p.Reception).ThenInclude(p => p.Patient)
                .Where(p => p.AnalysisResultMaster.Reception.PatientId == patientId && analysisItems.Contains(p.AnalysisItemId) && p.AnalysisResultMaster.InvoiceDate <= invoiceDate)
                .Select(p => new AnalysisResult
                {
                    Value = p.Value,
                    AnalysisItem = new AnalysisItem
                    {
                        Name = p.AnalysisItem.Name
                    },
                    AnalysisResultMaster = new AnalysisResultMaster
                    {
                        InvoiceDate=p.AnalysisResultMaster.InvoiceDate,
                    }
                });
        }
    }
}
