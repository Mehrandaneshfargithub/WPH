using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface ICostRepository : IRepository<Cost>
    {
        IEnumerable<Cost> GetAllCosts(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, Guid? costType);
        List<Cost> GetAllCostsForReport(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo);
        Cost GetWithType(Guid itemId);
        IEnumerable<Cost> GetAllPurchasInvoiceCosts(Guid purchaseInvoiceId);
        IEnumerable<Cost> GetAllSaleInvoiceCosts(Guid saleInvoiceId);
    }
}
