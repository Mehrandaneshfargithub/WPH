using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class AnalysisItemWorker : BackgroundService 
    {
        private readonly ILogger<AnalysisItemWorker> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AnalysisItemWorker(ILogger<AnalysisItemWorker> logger, IMemoryCache memoryCache, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _memoryCache = memoryCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //await Task.Delay(10, stoppingToken);
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                

            //    var CAnalysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
            //    if (CAnalysisItem is null)
            //    {
            //        var AnalysisItems = _unitOfWork.AnalysisItem.GetAllAnalysisItemAsync().ToList();
            //        _memoryCache.Set("analysisItems", AnalysisItems);
            //    }

            //    //_memoryCache.Set("analysisItems", 1);
            //    var CAnalysis = _memoryCache.Get<List<Analysis>>("analysis");
            //    if (CAnalysis is null)
            //    {
            //        var Analysis = _unitOfWork.Analysis.GetAllAnalysisWithAnalysisItem().ToList();
            //        var CachAnalysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
            //        foreach (var analis in Analysis)
            //            foreach(var item in analis.AnalysisAnalysisItems)
            //            {
            //                item.AnalysisItem = new AnalysisItem();
            //                item.AnalysisItem = CachAnalysisItem.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
            //            }

            //        _memoryCache.Set("analysis", Analysis);
            //    }

            //    var CGroupAnalysis = _memoryCache.Get<List<GroupAnalysis>>("groupAnalysis");
            //    if (CGroupAnalysis is null)
            //    {
            //        var GroupAnalysis = _unitOfWork.GroupAnalysis.GetAllGroupAnalysisWithAnalysisAndAnalysisItem().ToList();
            //        var CachAnalysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
            //        var CachAnalysis = _memoryCache.Get<List<Analysis>>("analysis");
            //        foreach (var gAnalis in GroupAnalysis)
            //        {
            //            foreach (var item in gAnalis.GroupAnalysisAnalyses)
            //            {
            //                item.Analysis = new Analysis();
            //                item.Analysis = CachAnalysis.SingleOrDefault(x => x.Guid == item.AnalysisId);
            //            }
            //            foreach (var item in gAnalis.GroupAnalysisItems)
            //            {
            //                item.AnalysisItem = new AnalysisItem();
            //                item.AnalysisItem = CachAnalysisItem.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
            //            }
            //        }

            //        _memoryCache.Set("groupAnalysis", GroupAnalysis);
            //    }


                
            //}
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(1000, stoppingToken);
            //}
        }
    }
}
