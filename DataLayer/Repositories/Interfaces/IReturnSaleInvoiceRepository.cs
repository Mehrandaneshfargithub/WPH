using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Interfaces
{
    public interface IReturnSaleInvoiceRepository : IRepository<ReturnSaleInvoice>
    {
        IEnumerable<ReturnSaleInvoice> GetAllReturnSaleInvoice(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Expression<Func<ReturnSaleInvoice, bool>> predicate = null);
        ReturnSaleInvoice GetForUpdateTotalPrice(Guid returnSaleInvoiceId);
        string GetLatestReturnSaleInvoiceNum(Guid clinicSectionId);
        //IEnumerable<PartialReceiveModel> GetNotPartialReceiveReturnSaleInvoice(Guid? supplierId, int? currencyId, Guid? receiveId);
        //IEnumerable<PartialReceiveModel> GetPartialReceiveReturnSaleInvoice(Guid? receiveId);
        IEnumerable<ReturnSaleInvoice> GetNotReceiveReturnSaleInvoice(Guid? supplierId);
        IEnumerable<ReturnSaleInvoice> GetReceiveReturnSaleInvoice(Guid? receiveId);
        IEnumerable<ReturnSaleInvoice> GetForReceive(IEnumerable<Guid> returnIds);
        ReturnSaleInvoice GetWithType(Guid returnSaleInvoiceId);
    }
}
