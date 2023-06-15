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
using WPH.MvcMockingServices;

namespace WPH.WorkerServices
{
    public class AnalysisItemWorker : BackgroundService 
    {
        private readonly ILogger<AnalysisItemWorker> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;
        public AnalysisItemWorker(ILogger<AnalysisItemWorker> logger, IMemoryCache memoryCache, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _memoryCache = memoryCache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
                
                var CAnalysisItem = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
                if (CAnalysisItem is null)
                {
                var scope = _serviceScopeFactory.CreateScope();
                var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

                _logger.LogInformation("Worker start: {time}", DateTimeOffset.Now);
                var AnalysisItems = await _unitOfWork.AnalysisItem.GetAllAnalysisItemAsync();

                _logger.LogInformation("Worker AnalysisItem at: {time}", DateTimeOffset.Now);
                var Analysis = await _unitOfWork.Analysis.GetAllAnalysisWithAnalysisItemAsync();

                _logger.LogInformation("Worker Analysis at: {time}", DateTimeOffset.Now);
                var GroupAnalysis = await _unitOfWork.GroupAnalysis.GetAllGroupAnalysisWithAnalysisAndAnalysisItemAsync();

                _logger.LogInformation("Worker GroupAnalysis at: {time}", DateTimeOffset.Now);

                List<AnalysisItem> AllAnalysisItems = AnalysisItems;
                List<Analysis> AllAnalysis = Analysis;
                List<GroupAnalysis> AllGroupAnalysis = GroupAnalysis;

                foreach (var analis in AllAnalysis)
                    foreach (var item in analis.AnalysisAnalysisItems)
                    {
                        item.AnalysisItem = new AnalysisItem();
                        item.AnalysisItem = AllAnalysisItems.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
                    }

                foreach (var gAnalis in AllGroupAnalysis)
                {
                    foreach (var item in gAnalis.GroupAnalysisAnalyses)
                    {
                        item.Analysis = new Analysis();
                        item.Analysis = AllAnalysis.SingleOrDefault(x => x.Guid == item.AnalysisId);
                    }
                    foreach (var item in gAnalis.GroupAnalysisItems)
                    {
                        item.AnalysisItem = new AnalysisItem();
                        item.AnalysisItem = AllAnalysisItems.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
                    }
                }

                _memoryCache.Set("analysisItems", AllAnalysisItems);
                _memoryCache.Set("analysis", AllAnalysis);
                _memoryCache.Set("groupAnalysis", AllGroupAnalysis);
            }

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(10000, stoppingToken);
            //    //await GetAnalysis();
            //}

        }

        public async Task GetAnalysis()
        {
            var scope = _serviceScopeFactory.CreateScope();
            var _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();

            _logger.LogInformation("Worker start: {time}", DateTimeOffset.Now);
            var AnalysisItems = await _unitOfWork.AnalysisItem.GetAllAnalysisItemAsync();

            _logger.LogInformation("Worker AnalysisItem at: {time}", DateTimeOffset.Now);
            var Analysis = await _unitOfWork.Analysis.GetAllAnalysisWithAnalysisItemAsync();

            _logger.LogInformation("Worker Analysis at: {time}", DateTimeOffset.Now);
            var GroupAnalysis = await _unitOfWork.GroupAnalysis.GetAllGroupAnalysisWithAnalysisAndAnalysisItemAsync();

            _logger.LogInformation("Worker GroupAnalysis at: {time}", DateTimeOffset.Now);

            List<AnalysisItem> AllAnalysisItems = AnalysisItems;
            List<Analysis> AllAnalysis = Analysis;
            List<GroupAnalysis> AllGroupAnalysis = GroupAnalysis;

            foreach (var analis in AllAnalysis)
                foreach (var item in analis.AnalysisAnalysisItems)
                {
                    item.AnalysisItem = new AnalysisItem();
                    item.AnalysisItem = AllAnalysisItems.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
                }

            foreach (var gAnalis in AllGroupAnalysis)
            {
                foreach (var item in gAnalis.GroupAnalysisAnalyses)
                {
                    item.Analysis = new Analysis();
                    item.Analysis = AllAnalysis.SingleOrDefault(x => x.Guid == item.AnalysisId);
                }
                foreach (var item in gAnalis.GroupAnalysisItems)
                {
                    item.AnalysisItem = new AnalysisItem();
                    item.AnalysisItem = AllAnalysisItems.SingleOrDefault(x => x.Guid == item.AnalysisItemId);
                }
            }

            _memoryCache.Set("analysisItems", AllAnalysisItems);
            _memoryCache.Set("analysis", AllAnalysis);
            _memoryCache.Set("groupAnalysis", AllGroupAnalysis);

        }


        public void RemoveCach()
        {
            _memoryCache.Remove("analysisItems");
            _memoryCache.Remove("analysis");
            _memoryCache.Remove("groupAnalysis");
        }

    }
}
