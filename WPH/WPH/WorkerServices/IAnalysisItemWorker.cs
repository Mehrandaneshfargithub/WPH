using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WPH.WorkerServices
{
    public interface IAnalysisItemWorker
    {
        Task StopAsync(CancellationToken token = default);
        void RemoveCach();
    }
}
