using DMSDataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DMSSaleInvoiceDetailMvcMockingService : IDMSSaleInvoiceDetailMvcMockingService
    {
        private readonly IDMSUnitOfWork _unitOfWork;
        public DMSSaleInvoiceDetailMvcMockingService(IDMSUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
