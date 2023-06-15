using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WPH.WorkerServices
{
    public class WorkerNew : BackgroundService
    {
        private readonly ILogger<AnalysisItemWorker> _logger;
        //private readonly IMemoryCache _memoryCache;
        //private readonly IServiceScopeFactory _serviceScopeFactory;
        public WorkerNew(ILogger<AnalysisItemWorker> logger/*, IMemoryCache memoryCache, IServiceScopeFactory serviceScopeFactory*/)
        {
            _logger = logger;
            //_serviceScopeFactory = serviceScopeFactory;
            //_memoryCache = memoryCache;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
